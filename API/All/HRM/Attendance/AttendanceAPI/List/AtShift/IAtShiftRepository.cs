using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.AtShift
{
    public interface IAtShiftRepository: IGenericRepository<AT_SHIFT, AtShiftDTO>
    {
        Task<FormatedResponse> SinglePhaseQueryList(GenericQueryListDTO<AtShiftDTO> request);
    }
}

