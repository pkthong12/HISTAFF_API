using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SeDocumentInfo
{
    public interface ISeDocumentInfoRepository : IGenericRepository<SE_DOCUMENT_INFO, SeDocumentInfoDTO>
    {
        Task<GenericPhaseTwoListResponse<SeDocumentInfoDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeDocumentInfoDTO> request);
        Task<FormatedResponse> GetByIdEmp(long id);
        Task<FormatedResponse> GetIdEmp(long id, long empId);
        Task<FormatedResponse> GetListDocument();
    }
}


