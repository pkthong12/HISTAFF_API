using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuCommend
{
    public interface ISysMailTemplateRepository: IGenericRepository<SYS_MAIL_TEMPLATE, SysMailTemplateDTO>
    {
       Task<GenericPhaseTwoListResponse<SysMailTemplateDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysMailTemplateDTO> request);
    }
}

