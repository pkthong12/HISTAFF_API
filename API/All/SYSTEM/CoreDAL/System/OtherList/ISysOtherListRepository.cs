using API.Entities;
using Common.Extensions;
using Common.Paging;
using CoreDAL.ViewModels;


namespace CoreDAL.Repositories
{
    public interface ISysOtherListRepository
    {
        Task<ResultWithError> CMSGetAllType(SysOtherListTypeDTO param);
        Task<PagedResult<SYS_OTHER_LIST>> CMSGetByType(SysOtherListDTO param);
        Task<ResultWithError> CreateTypeAsync(SysOtherListTypeInputDTO param);
        Task<ResultWithError> UpdateTypeAsync(SysOtherListTypeInputDTO param);
        Task<ResultWithError> ChangeStatusTypeAsync(List<long> ids);
        Task<ResultWithError> CreateAsync(SysOtherListInputDTO param);
        Task<ResultWithError> UpdateAsync(SysOtherListInputDTO param);
        Task<ResultWithError> ChangeStatusAsync(List<long> ids);
        Task<ResultWithError> GetOtherListByType(string typeCode);
        Task<ResultWithError> GetSysConfixByType(string typeCode);

    }
}
