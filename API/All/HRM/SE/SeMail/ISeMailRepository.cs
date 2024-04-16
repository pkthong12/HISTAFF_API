using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SeMail
{
    public interface ISeMailRepository: IGenericRepository<SE_MAIL, SeMailDTO>
    {
       Task<GenericPhaseTwoListResponse<SeMailDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeMailDTO> request);
       
    }
}

