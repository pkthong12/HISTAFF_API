using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrCourse
{
    public interface ITrCourseRepository: IGenericRepository<TR_COURSE, TrCourseDTO>
    {
       Task<GenericPhaseTwoListResponse<TrCourseDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrCourseDTO> request);
        Task<FormatedResponse> CreateNewCode();

        Task<FormatedResponse> GetListCourse();
    }
}

