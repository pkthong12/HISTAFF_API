using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class TrResultEvaluationDTO : BaseDTO
    {
        public long? TrSettingCriDetailId { get; set; }
        public int? PointEvaluate { get; set; }
        public string? GeneralOpinion { get; set; }
        public string? Question1 { get; set; }
        public string? Question2 { get; set; }
        public string? Question3 { get; set; }
        public string? Question4 { get; set; }
        public long? TrAssessmentResultId { get; set; }



        public string? CriteriaCode { get; set; }
        public string? CriteriaName { get; set; }
        public double? Ratio { get; set; }
        public double? PointMax { get; set; }
    }
}