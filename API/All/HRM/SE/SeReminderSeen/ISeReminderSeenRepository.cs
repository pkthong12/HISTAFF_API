using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SeReminderSeen
{
    public interface ISeReminderSeenRepository: IGenericRepository<SE_REMINDER_SEEN, SeReminderSeenDTO>
    {
       Task<GenericPhaseTwoListResponse<SeReminderSeenDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeReminderSeenDTO> request);
       Task<FormatedResponse> InsertReminderSeen(SeReminderSeenDTO request);
    }
}

