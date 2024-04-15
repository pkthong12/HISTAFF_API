using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuCommendEmployee
{
    public interface IHuCommendEmployeeRepository: IGenericRepository<HU_COMMEND_EMPLOYEE, HuCommendEmployeeDTO>
    {
       Task<GenericPhaseTwoListResponse<HuCommendEmployeeDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCommendEmployeeDTO> request);
       Task<FormatedResponse> ApproveCommend(List<long> Ids);
       Task<FormatedResponse> OpenApproveCommend(List<long> Ids);
    }
}

