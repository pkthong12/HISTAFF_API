using Common.Paging;

namespace ProfileDAL.ViewModels
{

    public class OrgChartRptViewDTO : Pagings
    {
        public decimal? Id { get; set; }
        public string Code { get; set; }
        public string orgName { get; set; }
        public string ParentName { get; set; }
        public decimal? ParentId { get; set; }
        public decimal? jobCnt { get; set; }
        public decimal? ytdFte { get; set; }
        public decimal? planFte { get; set; }
        public decimal? vsplanFte { get; set; }
        public string ownerName { get; set; }
        public bool? isJob { get; set; }
        public bool? nhomDuan { get; set; }
        public string fullName { get; set; }
    }
    
   public class OrgChartRptOutDTO
    {
        public decimal? Id { get; set; }
        public string Code { get; set; }
        public string orgName { get; set; }
        public string ParentName { get; set; }
        public long? ParentId { get; set; }
        public int? jobCnt { get; set; }
        public int? ytdFte { get; set; }
        public int? planFte { get; set; }
        public int? vsplanFte { get; set; }
        public string ownerName { get; set; }
        public bool? isJob { get; set; }
        public bool? nhomDuan { get; set; }
        public string fullName { get; set; }
    }
   
    public class OrgChartRptInputDTO
    {
        public decimal? Id { get; set; }
        public string Code { get; set; }
        public string orgName { get; set; }
        public string ParentName { get; set; }
        public long? ParentId { get; set; }
        public int? jobCnt { get; set; }
        public int? ytdFte { get; set; }
        public int? planFte { get; set; }
        public int? vsplanFte { get; set; }
        public string ownerName { get; set; }
        public bool? isJob { get; set; }
        public bool? nhomDuan { get; set; }
        public string fullName { get; set; }
        public string language { get; set; }
    }
}
