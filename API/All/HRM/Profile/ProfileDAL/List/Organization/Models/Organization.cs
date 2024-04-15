namespace ProfileDAL.Models
{
    /*
    [Table("HU_ORGANIZATION")]
    public class Organization : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public  int TENANT_ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string CODE { get; set; }

        [Required]
        [MaxLength(100)]
        public string NAME { get; set; }
        [ForeignKey("Parent")]
        public  int? PARENT_ID { get; set; }

        public  long? MNG_ID { get; set; } // Người quản lý

        public DateTime? FOUNDATION_DATE { get; set; } // Ngày thành lập

        public DateTime? DISSOLVE_DATE { get; set; }  // Ngày giải thể
        [MaxLength(100)]
        public string PHONE { get; set; }
        [MaxLength(100)]
        public string FAX { get; set; }
        [MaxLength(150)]
        public string ADDRESS { get; set; }

        [MaxLength(150)]
        public string BUSINESS_NUMBER { get; set; } // Số giấy phép kinh doanh

        public DateTime? BUSINESS_DATE { get; set; } // Ngày cấp phép kinh doanh
        [MaxLength(150)]
        public string TAX_CODE { get; set; } // Mã só thuế

        [MaxLength(1500)]
        public string NOTE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Organization Parent { get; set; }
        public List<Organization> Childs { get; set; }
        public string SHORT_NAME { get; set; }
        public int? LEVEL_ORG { get; set; }
    }
    */
}
