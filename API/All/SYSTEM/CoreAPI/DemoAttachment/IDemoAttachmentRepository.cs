using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.DemoAttachment
{
    public interface IDemoAttachmentRepository : IGenericRepository<DEMO_ATTACHMENT, DemoAttachmentDTO>
    {
        Task<GenericPhaseTwoListResponse<DemoAttachmentDTO>> SinglePhaseQueryList(GenericQueryListDTO<DemoAttachmentDTO> request);
        Task<FormatedResponse> GetAttachmentStatusList();
    }
}

