using API.Main;
using CORE.Services.File;

namespace API.DTO
{
    public class SeDocumentInfoDTO : BaseDTO
    {
        public long? EmpId { get; set; }
        public string? CodeEmp { get; set; }
        public string? NameEmp { get; set; }
        public string? OrgName { get; set; }
        public string? PosName { get; set; }
        public long? DocumentId { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentType { get; set; }
        public bool? IsObligatory { get; set; }
        public bool? IsPermissveUpload { get; set; }
        public DateTime? SubDate { get; set; }
        public bool? IsSubmit { get; set; }
        public string? Note { get; set; }
        public string? AttachedFile { get; set; }
        public AttachmentDTO? AttachedFileBuffer { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }


    }
}
