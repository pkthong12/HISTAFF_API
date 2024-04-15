using Common.Extensions;

namespace Common.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(int id);
        Task<ResultWithError> Add(TEntity entity);
        Task<int> AddRange(IEnumerable<TEntity> entities);
        Task<int> Update(TEntity entity);
        Task<int> Remove(TEntity entity);
        Task<string> UploadBase64(string imgBase64);
    }
}
