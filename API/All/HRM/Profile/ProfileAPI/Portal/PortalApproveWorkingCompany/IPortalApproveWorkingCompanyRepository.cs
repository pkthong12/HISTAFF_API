using API.DTO;
using API.DTO.PortalDTO;
using API.Entities.PORTAL;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalApproveHuWorking
{
    public interface IPortalApproveWorkingCompanyRepository : IGenericRepository<PORTAL_REQUEST_CHANGE, PortalRequestChangeDTO>
    {
        Task<GenericPhaseTwoListResponse<PortalRequestChangeDTO>> SinglePhaseQueryList(GenericQueryListDTO<PortalRequestChangeDTO> request);


        // lấy tất cả bản ghi trong PORTAL_REQUEST_CHANGE
        // rồi kết hợp với các dữ liệu khác
        Task<GenericPhaseTwoListResponse<PortalRequestChangeDTO>> GetAllRecord(GenericQueryListDTO<PortalRequestChangeDTO> request);


        // phê duyệt bản ghi 
        Task<FormatedResponse> ApproveHuWorking(GenericToggleIsActiveDTO model);


        // từ chối phê duyệt bản ghi 
        Task<FormatedResponse> UnapproveHuWorking(GenericUnapprovePortalDTO model);
    }
}

