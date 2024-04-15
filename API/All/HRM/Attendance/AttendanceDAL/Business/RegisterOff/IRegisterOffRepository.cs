using AttendanceDAL.ViewModels;
using Common.Interfaces;
using Common.Paging;
using Common.Extensions;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public interface IRegisterOffRepository : IRepository<AT_REGISTER_OFF>
    {
        Task<PagedResult<RegisterOffDTO>> GetAll(RegisterOffDTO param);
        Task<PagedResult<RegisterOffDTO>> GetAllOTEL(RegisterOffDTO param);
        Task<ResultWithError> CreateAsync(RegisterOffInputDTO param, int type);
        Task<ResultWithError> UpdateAsync(RegisterOffInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids, int StatusId);
        Task<ResultWithError> Delete(List<long> ids);
        Task<ResultWithError> PortalReg(RegisterOffInputDTO param, int type);
        Task<ResultWithError> PortalApprove(PortalApproveDTO param, int type, int status);
        Task<ResultWithError> PortalWaitList(int type);

        Task<ResultWithError> PortalHistoryBy(int id, int type);
        /// <summary>
        /// Portal GetBy ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>   
        Task<ResultWithError> PortalAppBy(int id, int type);
        Task<ResultWithError> PortalAppGetBy(int id, int type);
        Task<PagedResult<HistoryRegDTO>> GetRegister(Pagings param);
        Task<PagedResult<AcceptDTO>> GetAccept(Pagings param);
        Task<PagedResult<AcceptDTO>> GetAppHistotyList(Pagings param);
        Task<ResultWithError> PortalAppHistoryBy(int id);
        /// <summary>
        /// Version 2
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedResult<HistoryRegView>> PortalListRegister(DateSearchParam param);
        Task<ResultWithError> PortalHistoryBy(int id);
        Task<ResultWithError> PortalCancel(long id);
        Task<ResultWithError> PortalOTGet();
        Task<ResultWithError> PortalDMVSGet();
        Task<ResultWithError> test(List<string> code);


    }
}
