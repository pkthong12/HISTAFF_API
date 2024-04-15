namespace API.All.HRM.OM.Infotype
{
    public class HrmInfotypeDTO
    {
        public long? Id { get; set; }
        public string? Code { get; set; }

        public string? NameCode { get; set; }

        public string? NameEn { get; set; }

        public string? NameVn { get; set; }

        public string? Description { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string? UpdateBy { get; set; }
    }
}
