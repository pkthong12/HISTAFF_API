using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Communist.HuComEmployeeMng
{
    public interface IHuComEmployeeMngRepository : IGenericRepository<HU_COM_EMPLOYEE_MNG, HuComEmployeeMngDTO>
    {
        Task<GenericPhaseTwoListResponse<HuComEmployeeMngDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuComEmployeeMngDTO> request);
    }
}