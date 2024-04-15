using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsWherehealth
{
    public interface IInsWherehealthRepository: IGenericRepository<INS_WHEREHEALTH, InsWherehealthDTO>
    {
       Task<GenericPhaseTwoListResponse<InsWherehealthDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsWherehealthDTO> request);
        Task<FormatedResponse> GetAllProvince();
        Task<FormatedResponse> GetAllDistrictByProvinceId(long  provinceId);
    }
}

