using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtWorksign
{
    public interface IAtWorksignRepository: IGenericRepository<AT_WORKSIGN, AtWorksignDTO>
    {
        Task<GenericPhaseTwoListResponse<AtWorksignDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtWorksignDTO> request);

        Task<FormatedResponse> GetList(AtWorksignDTO param);

        Task<FormatedResponse> GetCurrentPeriodSalary();

        Task<FormatedResponse> DeleteWorksigns(GenericUnitOfWork _uow, AtWorksignDTO dto);

        Task<FormatedResponse> GetEmployeeInfo(AtWorksignDTO param);

        Task<FormatedResponse> GetShiftDefault(AtWorksignDTO param);

    }
}

