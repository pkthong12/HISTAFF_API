using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SysOtherList
{
    public interface ISysOtherListRepository: IGenericRepository<SYS_OTHER_LIST, SysOtherListDTO>
    {
       Task<GenericPhaseTwoListResponse<SysOtherListDTO>> SinglePhaseQueryList(GenericQueryListDTO<SysOtherListDTO> request);
       Task<FormatedResponse> GetAllGroupOtherListType();
       Task<FormatedResponse> GetOtherListByType(string  typeCode);
       Task<FormatedResponse> GetAllCommendObjByKey(long id);
       Task<FormatedResponse> GetAllStatusByKey(long id);
       Task<FormatedResponse> GetAllSourceByKey(long id);
       Task<FormatedResponse> GetAllGender(long id);
       Task<FormatedResponse> GetAllWelfareByKey(long id);
       Task<FormatedResponse> GetStatusCommend();
       Task<FormatedResponse> GetStatusApproveHuFamilyEdit();
       Task<FormatedResponse> GetEvaluateType();
       Task<FormatedResponse> GetIdStatusByCode(string code);
    }
}

