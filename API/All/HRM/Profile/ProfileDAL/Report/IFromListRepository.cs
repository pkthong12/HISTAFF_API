using ProfileDAL.ViewModels;
using Common.Interfaces;
using Common.Extensions;
using API.Entities;
using CORE.DTO;

namespace ProfileDAL.Repositories
{
    public interface IFormListRepository : IRepository<HU_FORM_LIST>
    {
        Task<ResultWithError> GetById(long id);
        Task<ResultWithError> GetTreeView();
        Task<ResultWithError> PrintForm(int id, int typeId);
        Task<ResultWithError> PrintFormProfile(int id);
        Task<ResultWithError> PrintFormSalary(PayrollInputDTO param);
        Task<ResultWithError> PrintFormAttendance(PayrollInputDTO param);
        Task<ResultWithError> UpdateAsync(FormListDTO param);
        /// <summary>
        /// Get list loi nhac
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResultWithError> GetListRemind();
        /// <summary>
        /// Get list loi nhac
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResultWithError> UpdateRemind(List<SettingRemindDTO> param);
        /// <summary>
        /// Get data cho popup remind
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResultWithError> GetRemind();
        Task<FormatedResponse> GetDashboard();
    }
}
