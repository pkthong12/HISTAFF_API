namespace API.All.Services
{
    public interface ISchedureService
    {
        Task<object> AddTestingRecordAt5AmEveryMorning();
        Task<object> TestNhanh();
        string DeveloperRooting();
        void ChangeIsActivePosition();
        Task<object> TheSystemChecksTheTerminate();
    }
}
