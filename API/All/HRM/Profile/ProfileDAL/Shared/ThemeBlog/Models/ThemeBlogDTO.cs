using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class ThemeBlogDTO : Pagings
    {
        public long? Id { get; set; }
        public string ImgUrl { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class ThemeBlogInputDTO
    {
        public long? Id { get; set; }
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

    }

}
