using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace CoreDAL.ViewModels
{
    public class SysModuleDTO : Pagings
    {
        public long Id { get; set; }
        public long? ApplicationId { get; set; }
        public string AppName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        //public string States { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }
        public int? Orders { get; set; }
    }
    public class SysModuleInputDTO
    {
        public long Id { get; set; }
        public long? ApplicationId { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string Code { get; set; }
        public string Note { get; set; }
        public string States { get; set; }
        public decimal Price { get; set; }
        public int? Orders { get; set; }
    }
}
