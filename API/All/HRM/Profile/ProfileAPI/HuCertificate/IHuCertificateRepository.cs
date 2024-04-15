using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuCertificate
{
    public interface IHuCertificateRepository: IGenericRepository<HU_CERTIFICATE, HuCertificateDTO>
    {
       Task<GenericPhaseTwoListResponse<HuCertificateDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCertificateDTO> request);
    }
}

