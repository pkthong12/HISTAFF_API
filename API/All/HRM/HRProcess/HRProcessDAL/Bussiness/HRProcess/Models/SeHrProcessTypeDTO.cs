using Common.Paging;

namespace HRProcessDAL.ViewModels
{
    public class SeHrProcessTypeDTO : Pagings
    {
        public Int64? Id { get; set; }
     
        public string Name { get; set; }
        public string Code { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedLog { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedLog { get; set; }
        public string StartBy { get; set; }
        public string OrderBy { get; set; }
        public string IconKey { get; set; }


    }

}
