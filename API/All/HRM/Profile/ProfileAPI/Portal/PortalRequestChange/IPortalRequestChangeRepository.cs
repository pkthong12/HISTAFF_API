using API.DTO.PortalDTO;
using API.Entities.PORTAL;
using CORE.DTO;
using CORE.GenericUOW;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace API.Controllers.PortalRequestChange
{
    public interface IPortalRequestChangeRepository : IGenericRepository<PORTAL_REQUEST_CHANGE, PortalRequestChangeDTO>
    {
        Task<GenericPhaseTwoListResponse<PortalRequestChangeDTO>> SinglePhaseQueryList(GenericQueryListDTO<PortalRequestChangeDTO> request);
        Task<FormatedResponse> Approve(GenericUnitOfWork _uow, PortalRequestChangeDTO dto, string sid, bool patchMode = true);
        Task<FormatedResponse> SendRequest(PortalRequestChangeDTO dto, string sid);
        Task<FormatedResponse> GetSalAllowanceProcessByEmp(long id);
        Task<FormatedResponse> GetConcurrentlyByEmpId(long id);
        Task<FormatedResponse> ApproveWorkingBeforeIds(List<long> id);
        Task<FormatedResponse> GetSalInsuByEmployeeId(long employeeId);
    }
}

