using Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRProcessDAL.Models
{
    [Table("SE_HR_NOTIFICATION")]
    public class SE_HR_NOTIFICATION : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string NAME { get; set; }

        [Required]
        [MaxLength(100)]
        public string CODE { get; set; }

        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }

        [MaxLength(450)]
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        [MaxLength(450)]
        public string MODIFIED_LOG { get; set; }
        [MaxLength(450)]
        public string START_BY { get; set; }
        [MaxLength(450)]
        public string ORDER_BY { get; set; }
        [MaxLength(450)]
        public string ICON_KEY { get; set; }
   
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
   
}
