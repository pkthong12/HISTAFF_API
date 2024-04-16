using API.Main;

namespace API.DTO
{
    public class TrCourseDTO : BaseDTO
    {
        public string? CourseCode { get; set; }
        public string? CourseName { get; set; }
        public int? CourseDate { get; set; }
        public decimal? Costs { get; set; }
        public string? Note { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public long? TrTrainField { get; set; }
        public string? TrTrainFieldName { get; set; }
        public string? ProfessionalTrainning { get; set; }
    }
}
