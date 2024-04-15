using Microsoft.AspNetCore.Mvc;
using AttendanceDAL.Repositories;
using AttendanceDAL.ViewModels;
using Common.Extensions;
using Common.Paging;

namespace AttendanceAPI.List
{
    [HiStaffAuthorize]
    [ApiExplorerSettings(GroupName = "3-ATTENDANCE-PORTAL")]
    [ApiController]

    [Route("api/hr/portal/[action]")]
    public class PortalController : BaseController2
    {
        public PortalController(IAttendanceUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        [HttpGet]
        public async Task<ActionResult> GetRegister(Pagings param)
        {
            var r = await _unitOfWork.RegisterOffRepository.GetRegister(param);
            return Ok(r);
        }
        [HttpGet]
        public async Task<ActionResult> GetAccept(Pagings param)
        {
            var r = await _unitOfWork.RegisterOffRepository.GetAccept(param);
            return Ok(r);
        }

        [HttpGet]
        public async Task<ActionResult> GetAppHistotyList(Pagings param)
        {
            var r = await _unitOfWork.RegisterOffRepository.GetAppHistotyList(param);
            return Ok(r);
        }
        /// <summary>
        /// /Chấm công qua GPRS
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<ActionResult> PortalUpTimeGPS([FromBody] SwipeDataGPRSDTO param)
        //{
        //    try
        //    {
        //        if (param.Latitude == null || param.Longitude == null)
        //        {
        //             return Ok(new ResultWithError("OUT_SIDE"));
        //        }
        //        // check valid radius

        //        var check = await _unitOfWork.TimeSheetDailyRepository.CheckDistance(double.Parse(param.Latitude), double.Parse(param.Longitude));
        //        if (check.StatusCode=="400")
        //        {
        //             return Ok(new ResultWithError("OUT_SIDE"));
        //        }

        //        if (!string.IsNullOrEmpty(param.Image))
        //        {                    
        //            param.Image = await _unitOfWork.TimeSheetDailyRepository.UploadBase64(param.Image);
        //            if (string.IsNullOrEmpty(param.Image))
        //            {
        //                return Ok(new ResultWithError("IMAGE_ERROR"));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new ResultWithError(ex.Message));
        //    }

        //    try
        //    {
        //        var r = await _unitOfWork.TimeSheetDailyRepository.PortalUpTimeGPRS(param);
        //        return Ok(r);
        //    }
        //    catch (Exception ex)
        //    {               
        //        return Ok(new ResultWithError(ex.Message));
        //    }
        //}

        [HttpGet]
        public async Task<ActionResult> RegOffHistoryBy(int id)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalHistoryBy(id, Consts.REGISTER_OFF);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult> LateEarlyHistoryBy(int id)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalHistoryBy(id, Consts.REGISTER_LATE);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<ActionResult> OverTimeHistoryBy(int id)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalHistoryBy(id, Consts.REGISTER_OT);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult> OverTimeAppBy(int id)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalAppBy(id, Consts.REGISTER_OT);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult> RegOffAppBy(int id)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalAppBy(id, Consts.REGISTER_OFF);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult> LateEarlyAppBy(int id)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalAppBy(id, Consts.REGISTER_LATE);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Detail for Approve
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> RegOffBy(int id)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalAppGetBy(id, Consts.REGISTER_OFF);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Detail for Approve
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> LateEarlyBy(int id)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalAppGetBy(id, Consts.REGISTER_LATE);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Detail for Approve
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> OverTimeBy(int id)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalAppGetBy(id, Consts.REGISTER_OT);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lịch sử phê duyệt GetBy ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>  
        [HttpGet]
        public async Task<ActionResult> AppHistoryBy(int id)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalAppHistoryBy(id);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// Version 2
        /// <summary>
        /// Lịch sử đăng ký
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>  
        [HttpGet]
        public async Task<ActionResult> ListRegister(DateSearchParam param)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalListRegister(param);
                return Ok(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xem chi tiết lịch sử
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>  
        [HttpGet]
        public async Task<ActionResult> RegisterBy(int id)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalHistoryBy(id);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xem chi tiết lịch sử
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>  
        [HttpPost]
        public async Task<ActionResult> RegisterCancel([FromBody]ReferParam param)
        {
            try
            {
                var r = await _unitOfWork.RegisterOffRepository.PortalCancel(param.Id);
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Current Entitlement (PHÉP NĂM HIỆN TẠI)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>  
        [HttpGet]
        public async Task<ActionResult> EntitlementCur()
        {
            try
            {
                var r = await _unitOfWork.DayOffYearRepository.PortalEntitlementCur();
                return ResponseResult(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult> EntilmentGet()
        {
            var r = await _unitOfWork.EntitlementEditRepository.PortalEntilmentGet();
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> OtGet()
        {
            var r = await _unitOfWork.RegisterOffRepository.PortalOTGet();
            return ResponseResult(r);
        }

        [HttpGet]
        public async Task<ActionResult> DMVSGet()
        {
            var r = await _unitOfWork.RegisterOffRepository.PortalDMVSGet();
            return ResponseResult(r);
        }
    }
}
