using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtSetupTimeEmp
{
    public interface IAtSetupTimeEmpRepository: IGenericRepository<AT_SETUP_TIME_EMP, AtSetupTimeEmpDTO>
    {
       Task<GenericPhaseTwoListResponse<AtSetupTimeEmpDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSetupTimeEmpDTO> request);
    }
}

