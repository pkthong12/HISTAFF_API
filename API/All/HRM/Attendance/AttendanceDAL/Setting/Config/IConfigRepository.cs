using AttendanceDAL.ViewModels;
using Common.Interfaces;
using Common.Extensions;
using API.Entities;

namespace AttendanceDAL.Repositories
{
    public interface IConfigRepository : IRepository<AT_CONFIG>
    {
        Task<ResultWithError> GetConfig();
        Task<ResultWithError> UpdateAsync(ConfigDTO param);
    }
}
