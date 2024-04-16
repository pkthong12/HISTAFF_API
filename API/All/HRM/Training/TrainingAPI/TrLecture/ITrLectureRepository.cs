using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.All.HRM.Training.TrainingAPI.TrLecture
{
    public interface ITrLectureRepository : IGenericRepository<TR_LECTURE, TrLectureDTO>
    {
        Task<GenericPhaseTwoListResponse<TrLectureDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrLectureDTO> request);
        Task<FormatedResponse> GetDropDownTrainingCenter();
        Task<FormatedResponse> GetByIdTrCenter(long id);
        Task<FormatedResponse> GetListTeacherByCenter(List<long> ids);
    }
}