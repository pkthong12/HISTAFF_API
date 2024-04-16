using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SeConfig
{
    public interface ISeConfigRepository: IGenericRepository<SE_CONFIG, SeConfigDTO>
    {
       Task<GenericPhaseTwoListResponse<SeConfigDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeConfigDTO> request);
        //Task<FormatedResponse> SendEmail(SeConfigDTO seConfig);
    }
}

