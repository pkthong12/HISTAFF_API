using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace API.DTO
{
    public class SeDocumentDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public long? DocumentType { get; set; }
        public string? DocumentTypeName { get; set; }
        public bool? IsObligatory { get; set; }
        public bool? IsPermissveUpload { get; set; }
        public string? Note { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdateLog { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get; set; }

    }
}
