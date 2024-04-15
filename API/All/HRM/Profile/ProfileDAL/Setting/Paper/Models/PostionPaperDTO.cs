using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class PostionPaperDTO : Pagings
    {
        public long Id { get; set; }
        public string PaperName { get; set; }
        public int PaperId { get; set; }
        public long? PosId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }


    public class PosPaperInputDTO
    {
        public int PosId { get; set; }
        public List<int> PaperId { get; set; }
    }

}
