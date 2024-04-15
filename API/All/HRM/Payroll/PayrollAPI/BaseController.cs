using CoreDAL.Common;
using Microsoft.AspNetCore.Mvc;
using Common.Extensions;
using PayrollDAL.Repositories;

namespace PayrollAPI
{
    public class BaseController : Controller
    {
        protected readonly IPayrollUnitOfWork _unitOfWork;
        //protected readonly ILogger _logger;
        public BaseController(
            IPayrollUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Return Error Code.
        /// </summary>
        protected JsonResult ResponseResult(string message)
        {
            return new JsonResult(new { message = message, statusCode = 400 }) { StatusCode = 200 };
        }

        /// <summary>
        /// Return Error Code.
        /// </summary>
        protected JsonResult ImportResult(ResultWithError obj)
        {
            return new JsonResult(new { data = obj.Data, message = obj.Error, statusCode = obj.StatusCode }) { StatusCode = int.Parse(obj.StatusCode) };
        }

        /// <summary>
        /// Return Result With Error Code.
        /// </summary>
        protected JsonResult ResponseResult(ResultWithError obj)
        {
            return new JsonResult(new { data = obj.Data, message = obj.Error, statusCode = obj.StatusCode }) { StatusCode = 200 };
        }

        protected JsonResult Error()
        {
            var validationErrors =
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray();
            return new JsonResult(new ErrorResponse { Message = validationErrors }) { StatusCode = 400 };
        }
        protected JsonResult ResponseValidation()
        {
            var validationErrors =
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray();
            return new JsonResult(new { Message = validationErrors, statusCode = 400 }) { StatusCode = 200 };
        }
    }
}
