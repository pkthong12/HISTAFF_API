using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrSettingCriCourse
{
    public interface ITrSettingCriCourseRepository: IGenericRepository<TR_SETTING_CRI_COURSE, TrSettingCriCourseDTO>
    {
       Task<GenericPhaseTwoListResponse<TrSettingCriCourseDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrSettingCriCourseDTO> request);
    }
}

