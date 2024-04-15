using Common.Paging;

namespace ProfileDAL.ViewModels
{

    public class JobChildTreeViewDTO : Pagings
    {
        public decimal? Id { get; set; }
        public string FunctionName { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public decimal? ParentId { get; set; }
    }
    public class JobChildTreeOutDTO
    {
        public decimal? Id { get; set; }
        public string FunctionName { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public decimal? ParentId { get; set; }
    }
    public class JobChildTreeInputDTO
    {
        public decimal? Id { get; set; }
        public string FunctionName { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public decimal? ParentId { get; set; }
        public string language { get; set; }
        public decimal? jobId { get; set; }
    }
}
