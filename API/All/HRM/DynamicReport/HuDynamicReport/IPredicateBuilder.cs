namespace API.All.HRM.DynamicReport.HuDynamicReport
{
    public interface IPredicateBuilder<out T>
    {
        Func<object, bool> BuildPredicate();
    }

    public class JsonExpressionParser<T> : IPredicateBuilder<T>
    {
        public Func<object, bool> BuildPredicate()
        {
            Func<T, bool> generatedPredicate = item => true;

            return obj => generatedPredicate((T)Convert.ChangeType(obj, typeof(T)));
        }
    }
}
