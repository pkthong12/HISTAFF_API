using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class ContractTypeSysViewDTO : Pagings
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Period{ get; set; }
        public int? DayNotice { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsLeave { get; set; } // có tính phép
    }
    public class ContractTypeSysOutputDTO
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Period { get; set; }
        public int? DayNotice { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }

    public class ContractTypeSysInputDTO
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Period { get; set; }
        public int? DayNotice { get; set; }
        public string Note { get; set; }
        public bool? IsLeave { get; set; } // có tính phép
    }

}
