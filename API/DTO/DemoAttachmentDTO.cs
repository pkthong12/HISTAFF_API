using API.Main;
using CORE.Services.File;

namespace API.DTO
{
    public class DemoAttachmentDTO : BaseDTO
    {
        public string? Name { get; set; }

        public string? FirstAttachment { get; set; }

        public string? SecondAttachment { get; set; }

        public long? StatusId { get; set; }

        public DateTime? EffectDate { get; set; }


        // EXTENDS:

        public string? StatusName { get; set; }

        public AttachmentDTO? FirstAttachmentBuffer { get; set; }

        public AttachmentDTO? SecondAttachmentBuffer { get; set; }
    }
}
