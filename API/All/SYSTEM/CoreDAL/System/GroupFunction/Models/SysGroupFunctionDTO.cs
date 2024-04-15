using Common.Paging;
namespace CoreDAL.ViewModels
{
    public class SysGroupFunctionDTO: Pagings
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long ApplicationId { get; set; }
        public string AppName { get; set; }
        public bool? IsActive { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class SysGroupFunctionInputDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long ApplicationId { get; set; }
    }
}
