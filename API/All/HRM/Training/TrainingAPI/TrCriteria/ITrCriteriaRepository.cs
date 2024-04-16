using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrCriteria
{
    public interface ITrCriteriaRepository: IGenericRepository<TR_CRITERIA, TrCriteriaDTO>
    {
       Task<GenericPhaseTwoListResponse<TrCriteriaDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrCriteriaDTO> request);

        Task<FormatedResponse> GetListCriteria();
    }
}

