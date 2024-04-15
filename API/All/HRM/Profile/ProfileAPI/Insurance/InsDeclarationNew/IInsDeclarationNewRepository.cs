using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.InsDeclarationNew
{
    public interface IInsDeclarationNewRepository : IGenericRepository<INS_DECLARATION_NEW, InsDeclarationNewDTO>
    {
        Task<GenericPhaseTwoListResponse<InsDeclarationNewDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsDeclarationNewDTO> request);
    }
}