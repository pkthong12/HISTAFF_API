using API.DTO;
using Common.Extensions;
using CORE.DTO;
using CORE.GenericUOW;
using ProfileDAL.ViewModels;

namespace API.Controllers.AtDecleareSeniority
{
    public interface IAtRegisterOverTimeRepository: IGenericRepository<AT_OVERTIME, AtOvertimeDTO>
    {
       //Task<FormatedResponse> SinglePhaseQueryList(GenericQueryListDTO<AtOvertimeDTO> request);
       Task<GenericPhaseTwoListResponse<AtOvertimeDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtOvertimeDTO> request);

        Task<FormatedResponse> CreateAsync(GenericUnitOfWork _uow, AtOvertimeDTO dto, string sid);

    }
}

