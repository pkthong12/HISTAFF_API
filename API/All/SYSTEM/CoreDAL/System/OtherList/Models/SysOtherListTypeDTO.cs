using Common.Paging;

namespace CoreDAL.ViewModels
{
    public class SysOtherListTypeDTO : Pagings
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
        public bool IsSystem { get; set; } = true;
    }
    public class SysOtherListTypeInputDTO
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? Orders { get; set; }
        public bool IsSystem { get; set; } = true;
    }
}
