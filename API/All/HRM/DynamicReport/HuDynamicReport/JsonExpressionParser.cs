using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using static System.Text.Json.JsonElement;

namespace API.All.HRM.DynamicReport.HuDynamicReport
{
    public class JsonExpressionParser
    {
        private const string StringStr = "String";

        private readonly string BooleanStr = nameof(Boolean);
        //private readonly string Int = nameof(Int32);
        private readonly string And = nameof(And).ToLower();
        private readonly string EQUAL = nameof(EQUAL);
        private readonly string GREATER_THAN = nameof(GREATER_THAN);
        private readonly string NOT_EQUAL = nameof(NOT_EQUAL);
        private readonly string STARTS_WITH = nameof(STARTS_WITH);
        private readonly string NOT_STARTS_WITH = nameof(NOT_STARTS_WITH);
        private readonly string CONTAINS = nameof(CONTAINS);
        private readonly string NOT_CONTAINS = nameof(NOT_CONTAINS);
        private readonly string ENDS_WITH = nameof(ENDS_WITH);
        private readonly string NOT_ENDS_WITH = nameof(NOT_ENDS_WITH);

        private readonly MethodInfo MethodContains = typeof(Enumerable).GetMethods(
                        BindingFlags.Static | BindingFlags.Public)
                        .Single(m => m.Name == nameof(Enumerable.Contains)
                            && m.GetParameters().Length == 2);

        private delegate Expression Binder(Expression left, Expression right);

        private Expression ParseTree<T>(
            JsonElement logicalOperator,
            ParameterExpression parm)
        {
            Expression left = null!;
            var gate = logicalOperator.GetProperty(nameof(logicalOperator)).GetString();

            JsonElement filters = logicalOperator.GetProperty(nameof(filters));

            Binder binder = gate == And ? (Binder)Expression.And : Expression.Or;

            Expression bind(Expression left, Expression right) =>
                left == null ? right : binder(left, right);

            foreach (var rule in filters.EnumerateArray())
            {
                if (rule.TryGetProperty(nameof(logicalOperator), out JsonElement check))
                {
                    var right = ParseTree<T>(rule, parm);
                    left = bind(left, right);
                    continue;
                }

                string relationalOperator = rule.GetProperty(nameof(relationalOperator)).GetString()!;
                string type = rule.GetProperty(nameof(type)).GetString()!;
                string name = rule.GetProperty(nameof(name)).GetString()!;

                JsonElement value = rule.GetProperty(nameof(value));

                var property = Expression.Property(parm, name);

                if (relationalOperator == STARTS_WITH)
                {
                    object val = (type == StringStr || type == BooleanStr) ?
                    (object)value.GetString()! : value.GetDecimal();
                    var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                    var startsWithExpression = Expression.Call(property, startsWithMethod!, Expression.Constant(value.GetString()));
                    left = bind(left, startsWithExpression);

                }
                else if (relationalOperator == CONTAINS)
                {
                    object val = (type == StringStr || type == BooleanStr) ?
                    (object)value.GetString()! : value.GetDecimal();
                    var startsWithMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var startsWithExpression = Expression.Call(property, startsWithMethod!, Expression.Constant(value.GetString()));
                    left = bind(left, startsWithExpression);
                }
                else if(relationalOperator == ENDS_WITH)
                {
                    object val = (type == StringStr || type == BooleanStr) ?
                    (object)value.GetString()! : value.GetDecimal();
                    var startsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                    var startsWithExpression = Expression.Call(property, startsWithMethod!, Expression.Constant(value.GetString()));
                    left = bind(left, startsWithExpression);
                }
                else if(relationalOperator == NOT_ENDS_WITH)
                {
                    object val = (type == StringStr || type == BooleanStr) ?
                    (object)value.GetString()! : value.GetDecimal();
                    var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                    var endsWithExpression = Expression.Call(property, endsWithMethod!, Expression.Constant(value.GetString()));
                    var notEndsWithExpression = Expression.Not(endsWithExpression);
                    left = bind(left, notEndsWithExpression);
                }
                else if(relationalOperator == EQUAL)
                {
                    object val = (type == StringStr || type == BooleanStr) ?
                        (object)value.GetString()! : value.GetDecimal();
                    var toCompare = Expression.Constant(val);
                    var right = Expression.Equal(property, toCompare);
                    left = bind(left, right);
                }
                else if(relationalOperator == NOT_EQUAL)
                {
                    object val = (type == StringStr || type == BooleanStr) ?
                        (object)value.GetString()! : value.GetDecimal();
                    var toCompare = Expression.Constant(val);
                    var right = Expression.NotEqual(property, toCompare);
                    left = bind(left, right);
                }
                else if(relationalOperator == NOT_STARTS_WITH)
                {
                    object val = (type == StringStr || type == BooleanStr) ?
                    (object)value.GetString()! : value.GetDecimal();
                    var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                    var startsWithExpression = Expression.Call(property, startsWithMethod!, Expression.Constant(value.GetString()));
                    var notStartsWithExpression = Expression.Not(startsWithExpression);
                    left = bind(left, notStartsWithExpression);
                }
                else if(relationalOperator == NOT_CONTAINS)
                {
                    object val = (type == StringStr || type == BooleanStr) ?
                    (object)value.GetString()! : value.GetDecimal();
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var containsExpression = Expression.Call(property, containsMethod!, Expression.Constant(value.GetString()));
                    var notcontainsExpression = Expression.Not(containsExpression);
                    left = bind(left, notcontainsExpression);
                }
            }

            return left;
        }

        public Expression<Func<T, bool>> ParseExpressionOf<T>(JsonDocument doc)
        {
            var itemExpression = Expression.Parameter(typeof(T));
            var conditions = ParseTree<T>(doc.RootElement, itemExpression);
            if(conditions != null)
            {
                if (conditions.CanReduce)
                {
                    conditions = conditions.ReduceAndCheck();
                }
            }
            else
            {
                return null!;
            }
            
            var query = Expression.Lambda<Func<T, bool>>(conditions, itemExpression);
            return query;
        }

        public Func<T, bool> ParsePredicateOf<T>(JsonDocument doc)
        {
            var query = ParseExpressionOf<T>(doc);
            if(query != null)
            {
                return query.Compile();
            }
            else
            {
                return null!;
            }
        }
    }
}
