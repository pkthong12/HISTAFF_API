using API.DTO.PortalDTO;
using API.Entities.PORTAL;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Profile.ProfileAPI.Portal.PortalCertificate;

public interface IPortalCertificateRepository : IGenericRepository<PORTAL_CERTIFICATE, PortalCertificateDTO>
{
    Task<GenericPhaseTwoListResponse<PortalCertificateDTO>> SinglePhaseQueryList(GenericQueryListDTO<PortalCertificateDTO> request);
    Task<FormatedResponse> GetListCertificate(long employeeId);
}

