namespace API.All.Services
{
    public interface IBackgroundService
    {
        Task InsertArising();
        Task ApproveWorking();
        Task ApproveTerminate();
        Task UpdateStatusEmpDetail();
    }
}
