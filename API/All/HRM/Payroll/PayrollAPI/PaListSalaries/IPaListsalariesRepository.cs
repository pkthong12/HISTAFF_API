using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.PaListsalaries
{
    public interface IPaListsalariesRepository: IGenericRepository<PA_LISTSALARIES, PaListsalariesDTO>
    {
       Task<GenericPhaseTwoListResponse<PaListsalariesDTO>> SinglePhaseQueryList(GenericQueryListDTO<PaListsalariesDTO> request);
        Task<FormatedResponse> GetObjSal();
        Task<FormatedResponse> GetGroupType();
        Task<FormatedResponse> GetDataType();
        Task<FormatedResponse> GetListSal(long idSymbol);
        Task<FormatedResponse> GetListSalaries(long idObjectSal);
        Task<FormatedResponse> GetListNameCode(PaListsalariesDTO param);
        Task<FormatedResponse> GetListObjSalry();
        Task<FormatedResponse> GetListObj(string typeCode);



    }
}

