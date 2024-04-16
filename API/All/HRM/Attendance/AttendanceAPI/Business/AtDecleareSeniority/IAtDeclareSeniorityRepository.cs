using API.DTO;
using Common.Extensions;
using CORE.DTO;
using CORE.GenericUOW;
using ProfileDAL.ViewModels;

namespace API.Controllers.AtDecleareSeniority
{
    public interface IAtDeclareSeniorityRepository: IGenericRepository<AT_REGISTER_LEAVE, AtDeclareSeniorityDTO>
    {
       Task<GenericPhaseTwoListResponse<AtDeclareSeniorityDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtDeclareSeniorityDTO> request);

    }
}

