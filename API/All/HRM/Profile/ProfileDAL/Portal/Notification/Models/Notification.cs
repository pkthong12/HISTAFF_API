namespace ProfileDAL.Models
{
    /*
    [Table("AT_NOTIFICATION")]// Thông báo
    public class Notification : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        [Required]
        public long TYPE { get; set; } // kiểu thông báo, là đăng ký nghỉ, đi muộn, ot, tin tức, khảo sát
        [Required]
        public long ACTION { get; set; } // đăng ký, phê duyệt , từ chối, tạo mới
        [Required]
        public long NOTIFI_ID { get; set; } // id bản ghi cần thông báo
        [Required]
        public long EMP_CREATE_ID { get; set; } // Người tạo ra thông báo
        [Required]
        public string FMC_TOKEN { get; set; } // fmc token cần gửi
        [Required]
        public string IS_READ { get; set; } // check đọc chưa
        [Required]
        public long TENANT_ID { get; set; } //
        [Required]
        public string TITLE{ get; set; } // title
        [Required]
        public string EMP_NOTIFY_ID { get; set; } // Người nhận thông báo
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
    */
}
