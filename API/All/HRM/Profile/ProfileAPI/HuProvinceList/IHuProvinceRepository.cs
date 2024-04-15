using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuProvinceList
{
    public interface IHuProvinceRepository: IGenericRepository<HU_PROVINCE, HuProvinceDTO>
    {
       Task<GenericPhaseTwoListResponse<HuProvinceDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuProvinceDTO> request);

		Task<FormatedResponse> CreateNewCode();
        Task<FormatedResponse> CheckActive(List<long> ids);


        //// phương thức submit áp dụng
        //Task<FormatedResponse> SubmitActivate(List<long>? Ids);


        //// phương thức submit ngừng áp dụng
        //Task<FormatedResponse> SubmitStopActivate(List<long>? Ids);
    }
}

