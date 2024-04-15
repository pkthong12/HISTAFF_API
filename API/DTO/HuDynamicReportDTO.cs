using API.Main;

namespace API.DTO
{
    public class HuDynamicReportDTO: BaseDTO
    {
        public string? ViewName { get; set; }
        public string? Form{ get; set; }
        public string? Json { get; set; }
        public string? Expression { get; set; }
        public string? ReportName { get; set; }
        public string? SelectedColumns { get; set; }
        public string? PrefixTrans { get; set; }
    }
}
