using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtSetupGps
{
    public interface IAtSetupGpsRepository: IGenericRepository<AT_SETUP_GPS, AtSetupGpsDTO>
    {
       Task<GenericPhaseTwoListResponse<AtSetupGpsDTO>> SinglePhaseQueryList(GenericQueryListDTO<AtSetupGpsDTO> request);
    }
}

