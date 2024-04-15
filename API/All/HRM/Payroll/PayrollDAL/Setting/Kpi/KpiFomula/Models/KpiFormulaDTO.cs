using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace PayrollDAL.ViewModels
{
    public class KpiFormulaDTO : Pagings
    {
        public long Id { get; set; }
        public string ColName { get; set; }
        public string Formula { get; set; }
        public int Orders { get; set; }
        public string Note { get; set; }
    }

    public class KpiFormulaInputDTO
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public string ColName { get; set; }
        public string Formula { get; set; }
        public int Orders { get; set; }
        public string Note { get; set; }
    }
    public class KpiFormulaCreateDTO
    {
        public long? Id { get; set; }
        public string ColName { get; set; }
        public string Formula { get; set; }
    }
}
