using Common.Paging;

namespace CoreDAL.ViewModels
{
    public class SysPermissionDTO : Pagings
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Orders { get; set; }
        public bool? IsActive { get; set; }
    }
    public class SysPermissionInputDTO
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Orders { get; set; }
    }
}
