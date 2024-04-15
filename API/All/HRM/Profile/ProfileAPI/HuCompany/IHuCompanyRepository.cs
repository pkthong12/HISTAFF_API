using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuCompany
{
    public interface IHuCompanyRepository: IGenericRepository<HU_COMPANY, HuCompanyDTO>
    {
        Task<GenericPhaseTwoListResponse<HuCompanyDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCompanyDTO> request);

        Task<bool> CheckActive(List<long> ids);

        Task<FormatedResponse> CreateNewCode();


        // phương thức submit áp dụng
        Task<FormatedResponse> SubmitActivate(List<long>? Ids);


        // phương thức submit ngừng áp dụng
        Task<FormatedResponse> SubmitStopActivate(List<long>? Ids);
    }
}

