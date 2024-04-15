using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SeAuthorizeApprove
{
    public interface ISeAuthorizeApproveRepository: IGenericRepository<SE_AUTHORIZE_APPROVE, SeAuthorizeApproveDTO>
    {
        Task<FormatedResponse> GetLevelOrder();
        Task<FormatedResponse> GetProcess();
        Task<FormatedResponse> GetLevelOrderById(long id);
        Task<GenericPhaseTwoListResponse<SeAuthorizeApproveDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeAuthorizeApproveDTO> request);
    }
}

