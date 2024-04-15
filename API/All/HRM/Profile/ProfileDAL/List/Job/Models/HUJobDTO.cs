using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class HUJobDTO
    {
        public List<HUJobFunctionDTO>? Child { get; set; }
        public long? Id { get; set; }
        public long? typeId { get; set; }
        public string? NameVN { get; set; }
        public string? NameEN { get; set; }
        public string? Code { get; set; }
        public string? Actflg { get; set; }
        public string? Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedLog { get; set; }
        public DateTime? ModifedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedLog { get; set; }
        public string? Request { get; set; }
        public string? Purpose { get; set; }
        public long? PhanLoaiID { get; set; }
        public string? JobBandID { get; set; }
        public string? JobFamilyID { get; set; }
        public List<long>? Ids { get; set; }
        public bool? ValueToBind { get; set; }
    }

    public class HUJobInputDTO : Pagings
    {
        public long Id { get; set; }
        public int? typeId { get; set; }
        
        public string NameVN { get; set; }
        public string NameEN { get; set; }
        public string Code { get; set; }
        public string Actflg { get; set; }
        public string ActflgStr { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedLog { get; set; }
        public DateTime? ModifedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedLog { get; set; }
        public string Request { get; set; }
        public string? Purpose { get; set; }
        public long? PhanLoaiID { get; set; }
        public string PhanLoaiName { get; set; }
        public string JobBandID { get; set; }
        public string JobFamilyID { get; set; }
        public long? LevelID { get; set; }
        public decimal? OrderNum { get; set; }
    }

    public class HUJobFunctionDTO
    {
        public long Id { get; set; }
        public long? JobID { get; set; }
        public string Name { get; set; }
        public string NameEN { get; set; }
        public long? ParentID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedLog { get; set; }
        public DateTime? ModifedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedLog { get; set; }
        public long? FunctionID { get; set; }
    }
    public class HUJobEditDTO
    {
        public long? Id { get; set; }
        public string? NameVn { get; set; }
        public string? NameVnNoCode { get; set; }
        public string? NameEn { get; set; }
        public string? Code { get; set; }
        public string? Actflg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedLog { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ModifiedLog { get; set; }
        public string? Request { get; set; }
        public string? Purpose { get; set; }
        public long? PhanLoaiId { get; set; }
        public long? JobBandId { get; set; }
        public long? JobFamilyId { get; set; }
        public string? Note { get; set; }
        public long? LevelId { get; set; }
        public decimal? Ordernum { get; set; }
    }

}
