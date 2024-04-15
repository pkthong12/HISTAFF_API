using Common.Paging;
namespace ProfileDAL.ViewModels
{
    public class GroupPositionSysDTO : Pagings
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long? AreaId { get; set; }
        public string AreaName { get; set; }
        public bool? IsActive { get; set; }
        public string Note { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class GroupPositionSysInputDTO
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public long? AreaId { get; set; }
    }
}
