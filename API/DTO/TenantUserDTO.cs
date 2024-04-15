using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Text.RegularExpressions;

namespace API.DTO
{
    public class TenantUserDTO
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public bool? IsPortal { get; set; }
        public bool? IsWebapp { get; set; }
        public bool? IsFirstLogin { get; set; }

        public long? GroupId { get; set; }
        public string? UserNameRef { get; set; }
        public string? Email { get; set; }
        public string? Fullname { get; set; }

        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? Avatar { get; set; }
        public long? EmpId { get; set; }
        public string? FcmToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? PasswordHash { get; set; }
        public string? Salt { get; set; }

        public bool? IsLock { get; set; }

        public bool? Del { get; set; }
        public string? DeviceId { get; set; }

        public bool? IsAdmin { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? TestString { get; set; }
        public long? TestOrgId { get; set; }
    }
}
