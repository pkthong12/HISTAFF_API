using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtSetupWifi
{
    public interface IAtSetupWifiRepository: IGenericRepository<AT_SETUP_WIFI, AtSetupWifiDTO>
    {
       Task<GenericPhaseTwoListResponse<AtSetupWifiDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSetupWifiDTO> request);
    }
}

