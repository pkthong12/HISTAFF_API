using API.All.DbContexts;
using API.Controllers.HuWelfareAuto;

namespace ProfileDAL.Repositories
{
    public interface IProfileUnitOfWork: IDisposable
    {
        ProfileDbContext DataContext { get; }
        Task<int> SaveChangesAsync();

     

        IGroupPositionRepository GroupPositionRepository { get; }
        IPositionRepository PositionRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IAllowanceRepository AllowanceRepository { get; }
        IWelfareRepository BenerfitRepository { get; }
        IAllowanceEmpRepository AllowanceEmpRepository { get; }
        ISalaryRepository SalaryRepository { get; }
        ISalaryScaleRepository SalaryScaleRepository { get; }
        ISalaryRankRepository SalaryRankRepository { get; }
        ISalaryLevelRepository SalaryLevelRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IThemeBlogRepository ThemeBlogRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IWorkingRepository WorkingRepository { get; }
        IContractRepository ContractRepository { get; }
        IContractAppendixRepository ContractAppendixRepository { get; }
        IHuWelfareAutoRepository WelfareAutoRepository { get; }

        ICommendRepository CommendRepository { get; }
        ITerminateRepository TerminateRepository { get; }
        IInsChangeRepository InsChangeRepository { get; }
        IInsuranceTypeRepository InsuranceTypeRepository { get; }
        IGroupPositionSysRepository GroupPositionSysRepository { get; }
        IPositionSysRepository PositionSysRepository { get; }
        IShiftSysRepository ShiftSysRepository { get; }
        IContractTypeSysRepository ContractTypeSysRepository { get; }
        IFormListRepository FormListRepository { get; }
        IBlogInternalRepository BlogInternalRepository { get; }
        IVoteRepository VoteRepository { get; }
        IUserOrganiRepository UserOrganiRepository { get; }
        ISettingMapRepository SettingMapRepository { get; }
        IReportRepository ReportRepository { get; }
        ISalarySysRepository SalarySysRepository { get; }
        ISalaryStructureSysRepository SalaryStructureSysRepository { get; }
        ISalaryElementSysRepository SalaryElementSysRepository { get; }
        IFormListSysRepository FormListSysRepository { get; }
        IRoomRepository RoomRepository { get; }
        IBookingRepository BookingRepository { get; }
        IPositionPaperRepository PositionPaperRepository { get; }
        IHuJobBandRepository HuJobBandRepository { get; }
        IHuJobRepository HuJobRepository { get; }
        ICandidateRepository CandidateRepository { get; }

	}
}
