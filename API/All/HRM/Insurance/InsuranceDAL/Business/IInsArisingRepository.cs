using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace InsuranceDAL.Repositories
{
    public interface IInsArisingRepository : IGenericRepository<INS_ARISING, InsArisingDTO>
    {
        Task<GenericPhaseTwoListResponse<InsArisingDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsArisingDTO> request);

        Task<bool> InsertArising(GenericUnitOfWork _uow, InsArisingDTO obj, string sid);

        Task<bool> InsertListArising(GenericUnitOfWork _uow, string sid);

        Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsArisingDTO dto, string sid);
    }
}

