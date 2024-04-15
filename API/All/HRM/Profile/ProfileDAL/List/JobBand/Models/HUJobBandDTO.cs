using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class HUJobBandDTO : Pagings
    {
        public long Id { get; set; }
        public string NameVN { get; set; }
        public string NameEN { get; set; }
        public string LevelFrom { get; set; }
        public string LevelTo { get; set; }
        public bool? Status { get; set; }
        public string StatusName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedLog { get; set; }
        public DateTime? ModifedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedLog { get; set; }
        public int? TitleGroupId { get; set; }
        public string TitleGroupName { get; set; }
        public int? Other { get; set; }
    }

    public class HUJobBandInputDTO 
    {
        public long? Id { get; set; }
        public string? NameVn { get; set; }
        public string? NameEn { get; set; }
        public string? LevelFrom { get; set; }
        public string? LevelTo { get; set; }
        public bool? Status { get; set; }
        public string? StatusName { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public string CreatedBy { get; set; }
        //public string CreatedLog { get; set; }
        //public DateTime? ModifedDate { get; set; }
        //public string ModifiedBy { get; set; }
        //public string ModifiedLog { get; set; }
        public int? TitleGroupId { get; set; }
        public string? TitleGroupName { get; set; }
        public int? Other { get; set; }
    }

}
