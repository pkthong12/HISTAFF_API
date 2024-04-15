using API.Main;
using CORE.Services.File;

namespace API.DTO
{
    public class SysUserDTO
    {
        public string? Id { get; set; }

        public string? Discriminator { get; set; }

        public long? GroupId { get; set; }

        public string? Fullname { get; set; }

        public bool? IsAdmin { get; set; }
        public bool? IsRoot { get; set; }

        public string? Avatar { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Username { get; set; }

        public string? Normalizedusername { get; set; }

        public string? Email { get; set; }

        public string? Normalizedemail { get; set; }

        public bool? Emailconfirmed { get; set; }
        public bool? IsLock { get; set; }
        public string? Passwordhash { get; set; }

        public string? Securitystamp { get; set; }

        public string? Concurrencystamp { get; set; }

        public string? Phonenumber { get; set; }

        public bool? Phonenumberconfirmed { get; set; }

        public bool? Twofactorenabled { get; set; }

        public DateTimeOffset? Lockoutend { get; set; }

        public bool? Lockoutenabled { get; set; }

        public int? Accessfailedcount { get; set; }
        public bool? IsFirstLogin { get; set; }
        public bool? IsPortal { get; set; }

        public bool? IsWebapp { get; set; }

        public long? EmployeeId { get; set; }

        // RELATED PROPERTIES:

        public string? CreatedByUsername { get; set; }
        public string? UpdatedByUsername { get; set; }

        public string? EmployeeCode { get; set; }

        public string? EmployeeName { get; set; }

        public string? GroupName { get; set; }
        public DateTime? LeaveJobDate { get; set; }
        public List<string>? UserIds{ get; set; }


        public AttachmentDTO? AttachmentBuffer { get; set; }

        public SysMutationLogBeforeAfterRequest? SysMutationLogBeforeAfterRequest { get; set; }
        public List<string>? ActualFormDeclaredFields { get; set; }

    }
}
