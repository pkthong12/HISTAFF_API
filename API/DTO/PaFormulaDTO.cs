using Microsoft.AspNetCore.Http.HttpResults;
using PayrollDAL.Models;

namespace API.DTO
{
    public class PaFormulaDTO
    {
        public long? Id { get; set; }
        public long? SalaryTypeId { get; set; }
        public string? ColName { get; set; }
        public string? Formula { get; set; }
        public string? FormulaName { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public int? Orders { get; set; }

        public bool? IsDaily { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
