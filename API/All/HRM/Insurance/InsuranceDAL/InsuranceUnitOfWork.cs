using API.All.DbContexts;
using CORE.GenericUOW;
using InsuranceDAL.Repositories;
using RegisterServicesWithReflection.Services.Base;

namespace InsuranceDAL
{
    [TransientRegistration]
    public class InsuranceUnitOfWork : IDisposable, IInsuranceUnitOfWork
    {
        readonly CoreDbContext _context;
        readonly GenericUnitOfWork _uow;
        public CoreDbContext DataContext { get { return _context; } }

        IInsArisingRepository _InsArisingRepository;

        private bool disposed = false;

        public InsuranceUnitOfWork(CoreDbContext context)
        {
            _context = context;
        }
        public IInsArisingRepository InsArisingRepository
        {
            get
            {
                if (_InsArisingRepository == null)
                    _InsArisingRepository = new InsArisingRepository(_context, _uow);

                return _InsArisingRepository;

            }
        }
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
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
