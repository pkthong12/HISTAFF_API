using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;

namespace API.All.Logger
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class RequestResponseLoggerErrorFilter : Attribute, IExceptionFilter
    {
        private RequestResponseLogModel GetLogModel(HttpContext context)
        {
            return context.RequestServices.GetService
                   <IRequestResponseLogModelCreator>().LogModel;
        }

        public void OnException(ExceptionContext context)
        {
            var model = GetLogModel(context.HttpContext);
            model.IsExceptionActionLevel = true;
            if (model.ResponseDateTimeUtcActionLevel == null)
            {
                model.ResponseDateTimeUtcActionLevel = DateTime.UtcNow;
            }
        }
    }
}
