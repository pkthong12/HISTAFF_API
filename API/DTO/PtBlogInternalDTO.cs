namespace API.DTO
{
    public class PtBlogInternalDTO
    {
        public long? Id { get; set; }
        public string? Title { get; set; }
        public string? ImgUrl { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }

        public long? ThemeId { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
