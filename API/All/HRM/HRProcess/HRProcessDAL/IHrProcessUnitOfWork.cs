namespace HRProcessDAL.Repositories
{
    public interface IHRProcessBusiness: IDisposable
    {
        Task<int> SaveChangesAsync();
        IHRProcessRepository HRProcessRepository { get; }
    }
}
