using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtSignDefault
{
    public interface IAtSignDefaultRepository : IGenericRepository<AT_SIGN_DEFAULT, AtSignDefaultDTO>
    {
        Task<GenericPhaseTwoListResponse<AtSignDefaultDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSignDefaultDTO> request);
    }
}

