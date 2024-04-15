using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsChange
{
    public interface IInsChangeRepository: IGenericRepository<INS_CHANGE, InsChangeDTO>
    {
       Task<FormatedResponse> SinglePhaseQueryList(GenericQueryListDTO<InsChangeDTO> request);

        Task<FormatedResponse> GetOtherListType();
        Task<FormatedResponse> GetInsTypeChange();
        Task<FormatedResponse> GetInforById(long id);
        Task<FormatedResponse> GetInschangeDashboard();
        Task<FormatedResponse> GetLstInsCheck(long id);
        Task<FormatedResponse> GetUnit(long id);
        Task<FormatedResponse> SpsInsArisingManualLoad(InsChangeDTO dto);
        Task<FormatedResponse> SpsInsArisingManualLoad2(InsChangeDTO dto);
        Task<FormatedResponse> SpsInsArisingManualGet(InsChangeDTO dto);
        Task<FormatedResponse> SpsInsArisingManualGet2(InsChangeDTO dto);

    }
}

