using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Training.TrainingAPI.TrTranningRecord
{
    public interface ITrTranningRecordRepository : IGenericRepository<TR_TRANNING_RECORD, TrTranningRecordDTO>
    {
        Task<GenericPhaseTwoListResponse<TrTranningRecordDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrTranningRecordDTO> request);
        Task<FormatedResponse> GetDropDownCourse();
    }
}