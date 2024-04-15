using API.DTO;
using Common.Extensions;
using CORE.DTO;
using CORE.GenericUOW;
using ProfileDAL.ViewModels;

namespace API.Controllers.AtRegisterLeave
{
    public interface IAtRegisterLeaveRepository: IGenericRepository<AT_REGISTER_LEAVE, AtRegisterLeaveDTO>
    {
       Task<GenericPhaseTwoListResponse<AtRegisterLeaveDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtRegisterLeaveDTO> request);

        Task<FormatedResponse> CreateAsync(GenericUnitOfWork _uow, AtRegisterLeaveDTO dto, string sid);

        Task<FormatedResponse> CreateVer2Async(GenericUnitOfWork _uow, DynamicDTO dto, string sid);
        Task<FormatedResponse> UpdateVer2(GenericUnitOfWork _uow, DynamicDTO dto, string sid, bool patchMode = true);

        Task<FormatedResponse> GetByIdVer2(long id);

    }
}

