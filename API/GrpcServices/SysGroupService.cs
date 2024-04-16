using API.All.DbContexts;
using API.All.SYSTEM.CoreAPI.Authorization;
using API.DTO;
using Grpc.Core;

namespace API.GrpcServices
{
    public class SysGroupService : GrpcSysGroup.GrpcSysGroupBase
    {
        private readonly ILogger<SysGroupService> _logger;
        private readonly FullDbContext _dbContext;
        public SysGroupService(ILogger<SysGroupService> logger, FullDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public override Task<QueryFunctionActionPermissionListReply> QueryFunctionActionPermissionList(QueryFunctionActionPermissionListRequest request, ServerCallContext context)
        {
            try
            {
                var raw = from gfa in _dbContext.SysGroupFunctionActions.AsNoTracking()
                                .Where(x => x.GROUP_ID == request.ObjectId)
                          from fa in _dbContext.SysFunctionActions.AsNoTracking().Where(x => x.FUNCTION_ID == gfa.FUNCTION_ID && x.ACTION_ID == gfa.ACTION_ID)
                          from f in _dbContext.SysFunctions.AsNoTracking().Where(x => x.ID == gfa.FUNCTION_ID).DefaultIfEmpty()
                          from m in _dbContext.SysModules.AsNoTracking().Where(x => x.ID == f.MODULE_ID).DefaultIfEmpty()
                          from a in _dbContext.SysActions.AsNoTracking().Where(x => x.ID == gfa.ACTION_ID).DefaultIfEmpty()

                          where fa != null && f.ROOT_ONLY != true

                          select new SysGroupFunctionActionDTO()
                          {
                              ModuleCode = m.CODE,
                              FunctionId = gfa.FUNCTION_ID,
                              FunctionCode = f.CODE,
                              ActionId = gfa.ACTION_ID,
                              ActionCode = a.CODE
                          };

                List<FunctionActionPermissionDTO> result = new();
                var list = raw.ToList();

                long? functionId = null;
                string moduleCode = "";
                string functionCode = "";
                string functionUrl = "";
                List<long> actionIds = new();
                List<string> actionCodes = new();
                list.ForEach(r =>
                {
                    moduleCode = r.ModuleCode ?? "";
                    functionCode = r.FunctionCode ?? "";
                    functionUrl = r.FunctionUrl ?? "";
                    if (r.FunctionId != functionId)
                    {
                        if (functionId != null)
                        {
                            result.Add(new FunctionActionPermissionDTO()
                            {
                                FunctionId = (long)functionId,
                                AllowedActionIds = actionIds,
                                ModuleCode = moduleCode,
                                FunctionCode = functionCode,
                                FunctionUrl = functionUrl,
                                AllowedActionCodes = actionCodes
                            });
                            actionIds = new();
                            actionCodes = new();
                            functionId = r.FunctionId;
                        }
                    }
                    functionId = r.FunctionId;
                    actionIds.Add((r.ActionId ?? 0));
                    actionCodes.Add(r.ActionCode ?? "");
                });
                // The tail
                if (functionId != null)
                {
                    result.Add(new FunctionActionPermissionDTO()
                    {
                        FunctionId = (long)functionId,
                        AllowedActionIds = actionIds,
                        ModuleCode = moduleCode,
                        FunctionCode = functionCode,
                        FunctionUrl = functionUrl,
                        AllowedActionCodes = actionCodes
                    });
                }
                return Task.FromResult(new QueryFunctionActionPermissionListReply() { StatusCode = 200, InnerBody = JsonConvert.SerializeObject(result) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Task.FromResult(new QueryFunctionActionPermissionListReply() { ErrorType = 2, MessageCode = ex.Message, StatusCode = 500 });
            }
        }
    }
}