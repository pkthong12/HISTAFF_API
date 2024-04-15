using Common.Paging;

namespace ProfileDAL.ViewModels
{

    public class JobPositionTreeViewDTO : Pagings
    {
        public decimal? Id { get; set; }
        public string Code { get; set; }
        public string orgName { get; set; }
        public string ParentName { get; set; }
        public decimal? ParentId { get; set; }
        public decimal? lyFte { get; set; }
        public decimal? totalEmp { get; set; }
        public decimal? chenhLech1 { get; set; }
        public decimal? totalPosition { get; set; }
        public decimal? chenhLech2 { get; set; }
        public bool? isJob { get; set; }
        public bool? nhomDuan { get; set; }
    }
    
   public class JobPositionTreeOutDTO
    {
        public decimal? Id { get; set; }
        public string Code { get; set; }
        public string orgName { get; set; }
        public string ParentName { get; set; }
        public decimal? ParentId { get; set; }
        public decimal? lyFte { get; set; }
        public decimal? totalEmp { get; set; }
        public decimal? chenhLech1 { get; set; }
        public decimal? totalPosition { get; set; }
        public decimal? checnhLech2 { get; set; }
        public bool? isJob { get; set; }
        public bool? nhomDuan { get; set; }
    }
   
    public class JobPositionTreeInputDTO
    {
        public decimal? Id { get; set; }
        public string Code { get; set; }
        public string orgName { get; set; }
        public string ParentName { get; set; }
        public decimal? ParentId { get; set; }
        public decimal? lyFte { get; set; }
        public decimal? totalEmp { get; set; }
        public decimal? chenhLech1 { get; set; }
        public decimal? totalPosition { get; set; }
        public decimal? checnhLech2 { get; set; }
        public bool? isJob { get; set; }
        public bool? nhomDuan { get; set; }
        public string language { get; set; }
    }
}
