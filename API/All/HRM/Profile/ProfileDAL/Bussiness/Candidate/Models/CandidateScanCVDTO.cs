using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class CandidateScanCVDTO : Pagings
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Fullname { get; set; }

        public string FileName { get; set; }
        public string Image { get; set; }
        public DateTime? BirthDate { get; set; }


        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int? GenderId { get; set; }
        public string GenderName { get; set; }
        public string Address { get; set; }
        public string Skill { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
    }

    public class CandidateScanCVInputDTO
    {
        public long? Id { get; set; }
        public string Fullname { get; set; }
        public string GenderName { get; set; }
        public string Address { get; set; }
        public string Skill { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Fullname_VN { get; set; }
        public string Gender { get; set; }

    }
    public class CandidateScanCVImportDTO
    {
        public IFormFile file { get; set; }
        public IWebHostEnvironment Environment { get; set; }
        public string scheme { get; set; }
        public string host { get; set; }
    }
    public class CandidateDTO
    {

    }
}
