using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.SeDocument
{
    public interface ISeDocumentRepository: IGenericRepository<SE_DOCUMENT, SeDocumentDTO>
    {
       Task<GenericPhaseTwoListResponse<SeDocumentDTO>> SinglePhaseQueryList(GenericQueryListDTO<SeDocumentDTO> request);
        public Task<FormatedResponse> CreateCode();
        public Task<FormatedResponse> GetDucumentType();

    }
}

