using API.All.DbContexts;
using AttendanceDAL.Repositories;
using RegisterServicesWithReflection.Services.Base;

namespace AttendanceDAL
{
    [TransientRegistration]
    public class AttendanceUnitOfWork : IDisposable, IAttendanceUnitOfWork
    {
        readonly AttendanceDbContext _context;

        public AttendanceDbContext DataContext { get { return _context; } }
        // List Progile

        // Dùng chung All Tenant

        // Nghiệp vụ
        ITimeTypeRepository _TimeTypeRepository;
        ISalaryPeriodRepository _SalaryPeriodRepository;
        //ITimeSheetDailyRepository _TimeSheetDailyRepository;
        ITimeLateEarlyRepository _TimeLateEarlyRepository;
        IRegisterOffRepository _RegisterOffRepository;
        ITimeSheetMonthlyRepository _TimeSheetMonthlyRepository;
        IDayOffYearRepository _DayOffYearRepository;
        IOverTimeRepository _OverTimeRepository;
        IEntitlementEditRepository _EntitlementEditRepository;
        IReportRepository _ReportRepository;
        IConfigRepository _ConfigRepository;
        IEntitlementRepository _EntitlementRepository;
        public AttendanceUnitOfWork(AttendanceDbContext context)
        {
            _context = context;
        }
        public IOverTimeRepository OverTimeRepository
        {
            get
            {
                if (_OverTimeRepository == null)
                    _OverTimeRepository = new OverTimeRepository(_context);

                return _OverTimeRepository;

            }
        }
        public IDayOffYearRepository DayOffYearRepository
        {
            get
            {
                if (_DayOffYearRepository == null)
                    _DayOffYearRepository = new DayOffYearRepository(_context);

                return _DayOffYearRepository;

            }
        }
        public ITimeSheetMonthlyRepository TimeSheetMonthlyRepository
        {
            get
            {
                if (_TimeSheetMonthlyRepository == null)
                    _TimeSheetMonthlyRepository = new TimeSheetMonthlyRepository(_context);

                return _TimeSheetMonthlyRepository;

            }
        }
        public IRegisterOffRepository RegisterOffRepository
        {
            get
            {
                if (_RegisterOffRepository == null)
                    _RegisterOffRepository = new RegisterOffRepository(_context);

                return _RegisterOffRepository;

            }
        }
        public ITimeLateEarlyRepository TimeLateEarlyRepository
        {
            get
            {
                if (_TimeLateEarlyRepository == null)
                    _TimeLateEarlyRepository = new TimeLateEarlyRepository(_context);

                return _TimeLateEarlyRepository;

            }
        }
        public ISalaryPeriodRepository SalaryPeriodRepository
        {
            get
            {
                if (_SalaryPeriodRepository == null)
                    _SalaryPeriodRepository = new SalaryPeriodRepository(_context);

                return _SalaryPeriodRepository;

            }
        }
        //public ITimeSheetDailyRepository TimeSheetDailyRepository
        //{
        //    get
        //    {
        //        if (_TimeSheetDailyRepository == null)
        //            _TimeSheetDailyRepository = new TimeSheetDailyRepository(_context);

        //        return _TimeSheetDailyRepository;

        //    }
        //}
        public ITimeTypeRepository TimeTypeRepository
        {
            get
            {
                if (_TimeTypeRepository == null)
                    _TimeTypeRepository = new TimeTypeRepository(_context);

                return _TimeTypeRepository;

            }
        }

        public IEntitlementEditRepository EntitlementEditRepository
        {
            get
            {
                if (_EntitlementEditRepository == null)
                    _EntitlementEditRepository = new EntitlementEditRepository(_context);
                return _EntitlementEditRepository;
            }
        }
         public IEntitlementRepository EntitlementRepository
        {
            get
            {
                if (_EntitlementRepository == null)
                    _EntitlementRepository = new EntitlementRepository(_context);
                return _EntitlementRepository;
            }
        }
        public IReportRepository ReportRepository
        {
            get
            {
                if (_ReportRepository == null)
                    _ReportRepository = new ReportRepository(_context);
                return _ReportRepository;
            }
        }

        public IConfigRepository ConfigRepository
        {
            get
            {
                if (_ConfigRepository == null)
                    _ConfigRepository = new ConfigRepository(_context);
                return _ConfigRepository;
            }
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
