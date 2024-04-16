using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class RcExamsDTO : BaseDTO
    {
        public long? OrgId { get; set; }
        public long? PositionId { get; set; }
        public string? Name { get; set; }
        public int? PointLadder { get; set; }
        public int? Coefficient { get; set; }
        public int? PointPass { get; set; }
        public int? ExamsOrder { get; set; }
        public bool? IsPv { get; set; }
        public string? Note { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
    }
}