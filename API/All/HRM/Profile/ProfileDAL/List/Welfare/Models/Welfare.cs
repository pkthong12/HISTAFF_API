namespace ProfileDAL.Models
{
    /*
    [Table("HU_WELFARE")] // Phúc lợi
    public class Welfare : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string CODE { get; set; }

        [Required]
        [MaxLength(100)]
        public string NAME { get; set; }

        public decimal? MONNEY { get; set; }
        public int? SENIORITY { get; set; }// thâm niên tháng
        public DateTime? DATE_START { get; set; }// ngày hiệu lực
        public DateTime? DATE_END { get; set; }//ngày hết hiệu 

        public bool? IS_ACTIVE { get; set; }

        [MaxLength(1500)]
        public string NOTE { get; set; }

        

        [MaxLength(450)]
        public string CREATED_BY { get; set; }

        [MaxLength(450)]
        public string UPDATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

    }
    [Table("HU_WELFARE_CONTRACT")]
    public class WelfareContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("WELFARE")]
        public int WELFARE_ID { get; set; }
        [ForeignKey("CONTRACT_TYPE")]
        public int CONTRACT_TYPE_ID { get; set; }
        
        public ContractType CONTRACT_TYPE { get; set; }
        public Welfare WELFARE { get; set; }

    }
    */
}
