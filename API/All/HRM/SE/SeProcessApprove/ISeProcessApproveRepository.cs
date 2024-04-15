using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SeProcessApprove
{
    public interface ISeProcessApproveRepository: IGenericRepository<SE_PROCESS_APPROVE, SeProcessApproveDTO>
    {
       Task<GenericPhaseTwoListResponse<SeProcessApproveDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeProcessApproveDTO> request);

        Task<FormatedResponse> GetLevelOrder();
        Task<FormatedResponse> GetListProcess();
        Task<FormatedResponse> GetListByProcess(long processId);
    }
}

