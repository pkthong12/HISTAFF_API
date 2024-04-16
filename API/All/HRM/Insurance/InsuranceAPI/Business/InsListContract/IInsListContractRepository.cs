using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsListContract
{
    public interface IInsListContractRepository: IGenericRepository<INS_LIST_CONTRACT, InsListContractDTO>
    {
       Task<GenericPhaseTwoListResponse<InsListContractDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsListContractDTO> request);
       //Task<FormatedResponse> GetListYearPeroid();
       Task<FormatedResponse> GetListYearPeroid();
       Task<FormatedResponse> GetInsListContract(int year);
    }
}

