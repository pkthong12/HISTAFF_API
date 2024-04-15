using Microsoft.AspNetCore.Mvc.Filters;

namespace API.All.Logger
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class RequestResponseLoggerActionFilter: Attribute, IActionFilter
    {
        private RequestResponseLogModel GetLogModel(HttpContext context)
        {
            return context.RequestServices.GetService
                   <IRequestResponseLogModelCreator>().LogModel;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var model = GetLogModel(context.HttpContext);
            model.RequestDateTimeUtcActionLevel = DateTime.UtcNow;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var model = GetLogModel(context.HttpContext);
            model.ResponseDateTimeUtcActionLevel = DateTime.UtcNow;
        }
    }
}
