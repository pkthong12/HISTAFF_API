using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Payroll.PayrollAPI.PaListSal
{
    public interface IPaListSalRepository : IGenericRepository<PA_LISTSAL, PaListSalDTO>
    {
        Task<GenericPhaseTwoListResponse<PaListSalDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaListSalDTO> request);
    }
}