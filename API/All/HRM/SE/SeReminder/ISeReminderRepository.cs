using API.DTO;
using Common.Extensions;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SeReminder
{
    public interface ISeReminderRepository: IGenericRepository<SE_REMINDER, SeReminderDTO>
    {
       Task<GenericPhaseTwoListResponse<SeReminderDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeReminderDTO> request);
        Task<FormatedResponse> GetRemind();
        Task<FormatedResponse> GetHistoryOrgId(long EmployeeId);
    }
}

