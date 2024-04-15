using Common.Paging;

namespace CoreDAL.ViewModels
{
    public class SysOtherListDTO : Pagings
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public long? TypeId { get; set; }
        public string? TypeName { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Note { get; set; }
    }

    public class SysOtherListInputDTO 
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public long? TypeId { get; set; }
        public int? Orders { get; set; }
    }
}
