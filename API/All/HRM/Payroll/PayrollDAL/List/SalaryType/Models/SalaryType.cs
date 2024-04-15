using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayrollDAL.Models
{
    [Table("HU_SALARY_TYPE")] // Bảng lương
    public class SalaryType
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
        public int? ORDERS { get; set; }
        public bool? IS_ACTIVE { get; set; }
        [MaxLength(1500)]
        public string NOTE { get; set; }
        
    }
}
