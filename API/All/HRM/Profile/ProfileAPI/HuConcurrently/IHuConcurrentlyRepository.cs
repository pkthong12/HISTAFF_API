using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuConcurrently
{
    public interface IHuConcurrentlyRepository : IGenericRepository<HU_CONCURRENTLY, HuConcurrentlyDTO>
    {
        Task<GenericPhaseTwoListResponse<HuConcurrentlyDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuConcurrentlyDTO> request);
        Task<GenericPhaseTwoListResponse<HuConcurrentlyDTO>> GetConcurrentEmployee(GenericQueryListDTO<HuConcurrentlyDTO> request);
        Task<FormatedResponse> GetEmployeeByConcurrentId(long id);
        Task<FormatedResponse> GetPositionPolitical();
        void TranferPosition();
        void ChangePositionPoliticalByDate();
        Task<FormatedResponse> GetAllConcurrentByEmployeeCvId(long employeeCvId);
    }
}

