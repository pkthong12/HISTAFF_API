using API.All.DbContexts;

namespace InsuranceDAL.Repositories
{
    public interface IInsuranceUnitOfWork: IDisposable
    {
        CoreDbContext DataContext { get; }
        Task<int> SaveChangesAsync();

        IInsArisingRepository InsArisingRepository { get; }
    }
}
