using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.TrReimbursement
{
    public interface ITrReimbursementRepository: IGenericRepository<TR_REIMBURSEMENT, TrReimbursementDTO>
    {
       Task<GenericPhaseTwoListResponse<TrReimbursementDTO>> SinglePhaseQueryList(GenericQueryListDTO<TrReimbursementDTO> request);
        Task<FormatedResponse> GetListProgram(TrReimbursementDTO param);
    }
}

