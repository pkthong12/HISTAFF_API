using Common.Paging;

namespace CoreDAL.ViewModels
{
    public class ApproveTemplateDTO : Pagings
    {
        public long Id { get; set; }
        public string TemplateName { get; set; }
        public long? TemplateType { get; set; }
        public int? TemplateOrder { get; set; }
        public string Actflg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedLog { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedLog { get; set; }
        public string TemplateCode { get; set; }

        //
        public string TemplateTypeName { get; set; }
    }
}
