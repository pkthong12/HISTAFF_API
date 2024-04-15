using API.All.DbContexts;
using API.DTO;
using Grpc.Core;

namespace API.GrpcServices
{
    public class SysFunctionService : GrpcSysFunction.GrpcSysFunctionBase
    {
        private readonly ILogger<SysFunctionService> _logger;
        private readonly FullDbContext _dbContext;
        public SysFunctionService(ILogger<SysFunctionService> logger, FullDbContext dbContext)
        {
            _logger =   logger;
            _dbContext = dbContext;
        }

        public override Task<ReadAllWithAllActionsReply> ReadAllWithAllActions(ReadAllWithAllActionsRequest request, ServerCallContext context)
        {
            try
            {
                var functions = (
                                from l in _dbContext.SysFunctions.AsNoTracking()
                                from m in _dbContext.SysModules.AsNoTracking().Where(x => x.ID == l.MODULE_ID)

                                    //where l.ROOT_ONLY != true // to be filtered on Frontend

                                select new SysFunctionDTO
                                {
                                    Id = l.ID,
                                    ModuleId = l.MODULE_ID,
                                    ModuleCode = m.CODE,
                                    GroupId = l.GROUP_ID,
                                    Code = l.CODE,
                                    RootOnly = l.ROOT_ONLY,
                                    Name = l.NAME,
                                    Path = l.PATH,
                                    PathFullMatch = l.PATH_FULL_MATCH,
                                    IsActive = l.IS_ACTIVE,
                                }).ToList();

                functions.ForEach(function =>
                {
                    List<string> codes = new();
                    var actions = from fa in _dbContext.SysFunctionActions.AsNoTracking().Where(x => x.FUNCTION_ID == function.Id)
                                  from a in _dbContext.SysActions.AsNoTracking().Where(x => x.ID == fa.ACTION_ID).DefaultIfEmpty()
                                  select new
                                  {
                                      code = a.CODE
                                  };
                    actions?.ToList().ForEach(a => codes.Add(a.code)); ;
                    function.actionCodes = codes;

                });
                return Task.FromResult(new ReadAllWithAllActionsReply() { StatusCode = 200, InnerBody = JsonConvert.SerializeObject(functions)});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Task.FromResult(new ReadAllWithAllActionsReply() { ErrorType = 2, MessageCode = ex.Message, StatusCode = 500 });
            }
        }
    }
}