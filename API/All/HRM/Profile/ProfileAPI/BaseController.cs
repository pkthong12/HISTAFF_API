using Microsoft.AspNetCore.Mvc;
using Common.Extensions;
using ProfileDAL.Repositories;
using PayrollDAL.Repositories;

namespace ProfileAPI
{
    public class BaseController1 : ControllerBase
    {

        protected readonly IProfileUnitOfWork _unitOfWork;
        protected readonly IPayrollUnitOfWork _payrollBusiness;
        //protected readonly ILogger _logger;
     
        public BaseController1(IProfileUnitOfWork profileBusiness)
        {
            _unitOfWork = profileBusiness;
        }
        public BaseController1(IProfileUnitOfWork profileBusiness, IPayrollUnitOfWork payrollBusiness)
        {
            _unitOfWork = profileBusiness;
            _payrollBusiness = payrollBusiness;

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
        protected JsonResult ResponseResult(int status)
        {
            return new JsonResult(new { }) { StatusCode = status };
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
            return new JsonResult(new { InnerBody = obj.InnerBody,data = obj.Data, message = obj.Error, Error = obj.Error, statusCode = obj.StatusCode }) { StatusCode = 200 };
        }
        protected JsonResult Response_Result(ResultWithError obj)
        {
            return new JsonResult(new { InnerBody = obj.InnerBody,data = obj.Data, message = obj.Error, Error = obj.Error, statusCode = int.Parse(obj.StatusCode) }) { StatusCode = 200 };
        }
        /// <summary>
        /// Return Result With Error Code.
        /// </summary>
        protected JsonResult ResponseResult(object obj)
        {
            return new JsonResult(new { data = obj, message = string.Empty, statusCode = 200 }) { StatusCode = 200 };
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
