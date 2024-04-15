using Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using API.Entities;

namespace CoreDAL.Models
{
    [Table("SYS_USER")]
    public class SysUser : IdentityUser, IAuditableEntity
    {
        [ForeignKey("SysGroupUser")]
        public long GROUP_ID { get; set; }
        [MaxLength(100)]
        public string FULLNAME { get; set; }
       
        [Required]
        public bool IS_ADMIN { get; set; }
        
        [MaxLength(150)]
        public string AVATAR { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public SYS_GROUP SysGroupUser { get; set; }
        //public bool IS_LOCK { get; set; }
        //public bool IS_WEBAPP { get; set; }
        //public bool IS_PORTAL { get; set; }
        //public bool IS_FIRST_LOGIN { get; set; }
        //public long? EMPLOYEE_ID { get; set; }
        //[MaxLength(450)]
        //public string? FCM_TOKEN { get; set; }
        //public string? USER_NAME_REF { get; set; }
        //[MaxLength(450)]
        //public string? DEVICE_ID { get; set; }
        //[MaxLength(100)]
        //public string? EMPLOYEE_CODE { get; set; }
        //[MaxLength(100)]
        //public string? EMPLOYEE_NAME { get; set; }
        //public bool IS_DEL { get; set; }
    }
    
}
