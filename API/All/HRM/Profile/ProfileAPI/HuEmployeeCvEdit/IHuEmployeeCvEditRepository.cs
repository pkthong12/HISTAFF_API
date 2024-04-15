using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuEmployeeCvEdit
{
    public interface IHuEmployeeCvEditRepository: IGenericRepository<HU_EMPLOYEE_CV_EDIT, HuEmployeeCvEditDTO>
    {
        Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEmployeeCvEditDTO> request);

        Task<FormatedResponse> ApproveEducationEdit(List<long>? listId);
        Task<FormatedResponse> GetHuEmployeeCvEditCvApproving(long employeeId);


        // chức năng lưu
        Task<FormatedResponse> SaveEducation(HuEmployeeCvEditDTO request);
    }
}

