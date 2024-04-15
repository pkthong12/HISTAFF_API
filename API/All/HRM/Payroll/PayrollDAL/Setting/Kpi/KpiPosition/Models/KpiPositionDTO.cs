using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace PayrollDAL.ViewModels
{
    public class KpiPositionDTO : Pagings
    {
        public long Id { get; set; }
        public long KpiGroupId { get; set; }
        public string KpiGroupName { get; set; }
        public long KpiTargetId { get; set; }
        public string KpiTargetName { get; set; }
        public long? PositionId { get; set; }
    }

    public class KpiPositionInputDTO
    {
        public long? Id { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public List<long> KpiTargetId { get; set; }
        [Required(ErrorMessage = "{0_Required}")]
        public int PositionId { get; set; }
    }

}
