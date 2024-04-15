using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuEmployeeCv
{
    public interface IPortalHuEmployeeCvRepository : IGenericRepository<HU_EMPLOYEE_CV, HuEmployeeCvDTO>
    {
        Task<GenericPhaseTwoListResponse<HuEmployeeCvDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEmployeeCvDTO> request);
        Task<FormatedResponse> GetContractDetail(long employeeId);
        Task<FormatedResponse> ComingSoonBirthdayList();
    }
}

