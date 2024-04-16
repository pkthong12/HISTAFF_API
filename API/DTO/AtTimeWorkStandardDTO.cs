using System.Drawing;
using API.Main;

namespace API.DTO
{
    public class AtTimeWorkStandardDTO : BaseDTO
    {
        public int? EffectiveYear { get; set; }
        public long? OrgId { get; set; }
        public long? ObjEmployeeId { get; set; }
        public long? WorkEnvironmentId { get; set; }
        public bool? IsNotSaturdayIncluded { get; set; }
        public bool? IsNotSundayIncluded { get; set; }
        public bool? IsNotHalfSaturdayIncluded { get; set; }
        public bool? IsNotTwoSaturdays { get; set; }
        public int? DeductWorkDuringMonth { get; set; }
        public int? DefaultWork { get; set; }
        public int? Coefficient { get; set; }
        public int? T1 { get; set; }
        public int? T2 { get; set; }
        public int? T3 { get; set; }
        public int? T4 { get; set; }
        public int? T5 { get; set; }
        public int? T6 { get; set; }
        public int? T7 { get; set; }
        public int? T8 { get; set; }
        public int? T9 { get; set; }
        public int? T10 { get; set; }
        public int? T11 { get; set; }
        public int? T12 { get; set; }
        public string? OrgName { get; set; }
        public string? ObjEmployeeName { get; set; }
        public string? WorkEnvironmentName { get; set; }
    }
}