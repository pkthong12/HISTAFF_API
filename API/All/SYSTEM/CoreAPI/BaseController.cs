using CoreDAL.Common;
using Microsoft.AspNetCore.Mvc;
using Common.Paging;
using Common.Extensions;
using CoreDAL.Repositories;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CoreAPI
{
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        public BaseController(IUnitOfWork unitOfWork)
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
        protected async Task<IActionResult> PagingList<T>(IQueryable<T> list, Pagings param)
        {
            int skip = (param.PageNo - 1) * param.PageSize;
            int count = await list.CountAsync();
            var q = list.Skip((param.PageNo - 1) * Consts.PAGE_SIZE).Take(Consts.PAGE_SIZE);
            var r = await q.ToListAsync();
            return Ok(new PagedResult<T>(r, param.PageNo, param.PageSize, count));
        }
        protected string TenatCode
        {
            get
            {
                if (RouteData.Values.ContainsKey("Tenat_code"))
                {
                    return RouteData.Values.First(p => p.Key.Equals("Tenat_code")).Value.ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        protected int TenatId
        {
            get
            {
                var TenatId = User.Claims.Where(p => p.Type.Equals(JwtRegisteredClaimNames.Sid)).FirstOrDefault();
                if (TenatId == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(TenatId.Value);
                }
            }
        }

        protected string UserId
        {
            get
            {
                var userId = User.Claims.Where(p => p.Type.Equals(JwtRegisteredClaimNames.Azp)).FirstOrDefault();
                if (userId == null)
                {
                    return string.Empty;
                }
                else
                {
                    return userId.Value;
                }
            }
        }

    }
}
