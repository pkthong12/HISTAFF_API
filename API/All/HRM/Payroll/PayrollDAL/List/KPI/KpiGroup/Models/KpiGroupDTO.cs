using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace PayrollDAL.ViewModels
{
    public class KpiGroupDTO : Pagings
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Orders { get; set; }
        public string Note { get; set; }
    }

    public class KpiGroupOutDTO 
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Orders { get; set; }
        public string Note { get; set; }
    }

    public class KpiGroupInputDTO
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public string Name { get; set; }
        public int Orders { get; set; }
        public string Note { get; set; }
    }

}
