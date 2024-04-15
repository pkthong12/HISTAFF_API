using Common.Paging;
namespace ProfileDAL.ViewModels
{
    public class RoomDTO : Pagings
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class RoomInputDTO
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public int? Orders { get; set; }
        public string Note { get; set; }
    }
}
