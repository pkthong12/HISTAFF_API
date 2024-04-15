using API.All.DbContexts;
using PayrollDAL.Repositories;

namespace PayrollDAL
{
    public class PayrollUnitOfWork : IPayrollUnitOfWork
    {
        readonly PayrollDbContext _context;
        // List
        ISalaryElementRepository _SalaryElementRepository;
        ISalaryStructureRepository _SalaryStructureRepository;
        // Dùng chung All
        IElementGroupRepository _ElementGroupRepository;
        //setting
        IFormulaRepository _FormulaRepository;
        // Nghiệp vụ
        IKpiGroupRepository _KpiGroupRepository;
        IKpiTargetRepository _KpiTargetRepository;
        IKpiFormulaRepository _KpiFormulaRepository;
        IKpiPositionRepository _KpiPositionRepository;
        IKpiEmployeeRepository _KpiEmployeeRepository;
        IAdvanceRepository _AdvanceRepository;
       
        IReportRepository _ReportRepository;
        ISalaryImportRepository _SalaryImportRepository;
        IPaycheckRepository _PaycheckRepository;
        // product
     
        public PayrollUnitOfWork(PayrollDbContext context)
        {
            _context = context;
        }
        public IKpiEmployeeRepository KpiEmployeeRepository
        {
            get
            {
                if (_KpiEmployeeRepository == null)
                    _KpiEmployeeRepository = new KpiEmployeeRepository(_context);

                return _KpiEmployeeRepository;

            }
        }
        public IKpiPositionRepository KpiPositionRepository
        {
            get
            {
                if (_KpiPositionRepository == null)
                    _KpiPositionRepository = new KpiPositionRepository(_context);

                return _KpiPositionRepository;

            }
        }
        public IKpiFormulaRepository KpiFormulaRepository
        {
            get
            {
                if (_KpiFormulaRepository == null)
                    _KpiFormulaRepository = new KpiFormulaRepository(_context);

                return _KpiFormulaRepository;

            }
        }
        public IKpiTargetRepository KpiTargetRepository
        {
            get
            {
                if (_KpiTargetRepository == null)
                    _KpiTargetRepository = new KpiTargetRepository(_context);

                return _KpiTargetRepository;

            }
        }
        public IKpiGroupRepository KpiGroupRepository
        {
            get
            {
                if (_KpiGroupRepository == null)
                    _KpiGroupRepository = new KpiGroupRepository(_context);

                return _KpiGroupRepository;

            }
        }
        public ISalaryElementRepository SalaryElementRepository
        {
            get
            {
                if (_SalaryElementRepository == null)
                    _SalaryElementRepository = new SalaryElementRepository(_context);

                return _SalaryElementRepository;

            }
        }
        public ISalaryStructureRepository SalaryStructureRepository
        {
            get
            {
                if (_SalaryStructureRepository == null)
                    _SalaryStructureRepository = new SalaryStructureRepository(_context);

                return _SalaryStructureRepository;

            }
        }

        public IFormulaRepository FormulaRepository
        {
            get
            {
                if (_FormulaRepository == null)
                    _FormulaRepository = new FormulaRepository(_context);

                return _FormulaRepository;

            }
        }
        public IAdvanceRepository AdvanceRepository
        {
            get
            {
                if (_AdvanceRepository == null)
                    _AdvanceRepository = new AdvanceRepository(_context);

                return _AdvanceRepository;

            }
        }
     
       
        // Shared
        public IElementGroupRepository ElementGroupRepository
        {
            get
            {
                if (_ElementGroupRepository == null)
                    _ElementGroupRepository = new ElementGroupRepository(_context);

                return _ElementGroupRepository;

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

        public ISalaryImportRepository SalaryImportRepository
        {
            get
            {
                if (_SalaryImportRepository == null)
                    _SalaryImportRepository = new SalaryImportRepository(_context);

                return _SalaryImportRepository;

            }
        }

        public IPaycheckRepository PaycheckRepository
        {
            get
            {
                if (_PaycheckRepository == null)
                    _PaycheckRepository = new PaycheckRepository(_context);

                return _PaycheckRepository;

            }
        }
    }
}
