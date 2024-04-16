using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class PeCapacityFrameworkDTO : BaseDTO
    {
        public int? RatioFrom { get; set; }
        public int? RatioTo { get; set; }
        public string? Rating { get; set; }
        public long? ScoreNotRequired { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }



        public string? ScoreNotRequiredStr { get; set; }
        public string? Status { get; set; }
    }
}