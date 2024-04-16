using API.All.DbContexts;
using API.DTO;
using CORE.GenericUOW;
using InsuranceDAL.Repositories;
using ProfileDAL.Repositories;
using Microsoft.Extensions.Options;
using RegisterServicesWithReflection.Services.Base;

namespace API.All.Services
{
    [TransientRegistration]
    public class BackgroundService: IBackgroundService
    {
        private readonly GenericUnitOfWork _uow;
        private IInsArisingRepository _insArisingRepository;
        private IWorkingRepository _workingRepository;
        private ITerminateRepository _terminateRepository;
        private IContractRepository _contractRepository;
        public BackgroundService(CoreDbContext coreDbContext, ProfileDbContext profileContext, ProfileDbContext context) 
        {
            _uow = new GenericUnitOfWork(coreDbContext);
            _insArisingRepository = new InsArisingRepository(coreDbContext, _uow);
            _workingRepository = new WorkingRepository(profileContext);
            _terminateRepository = new TerminateRepository(profileContext);
            _contractRepository = new ContractRepository(context);
        }

        public async Task InsertArising()
        {
            var r = await _insArisingRepository.InsertListArising(_uow, "9aad23c2-2a73-4344-b534-e5dd6d2e1132");
        }

        public async Task ApproveWorking()
        {
            var r = await _workingRepository.ApproveListWorking("9aad23c2-2a73-4344-b534-e5dd6d2e1132");
        }

        public async Task ApproveTerminate()
        {
            var r = await _terminateRepository.ApproveList("9aad23c2-2a73-4344-b534-e5dd6d2e1132");
        }   
        
        public async Task UpdateStatusEmpDetail()
        {
            var r = await _contractRepository.ApproveList("9aad23c2-2a73-4344-b534-e5dd6d2e1132");
        }        
    }
}
