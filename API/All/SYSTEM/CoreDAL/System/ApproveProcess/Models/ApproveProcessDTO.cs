using Common.Paging;

namespace CoreDAL.ViewModels
{
    public class ApproveProcessDTO : Pagings
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Actflg { get; set; }
        public string ProcessCode { get; set; }
        public int? NumRequest { get; set; }
        public string Email { get; set; }
        public bool? IsSendEmail { get; set; }
    }
}
