using API.Main;

namespace API.DTO
{
    public class AtOrgPeriodDTO:BaseDTO
    {
        public long? OrgId { get; set; }
        public long? PeriodId { get; set; }
        public int?  Statuscolex { get; set; }
        public int?  StatuscolexSub { get; set; }
        public int?  StatuscolexBackdate { get; set; }
        public int?  Statusparox { get; set; }
        public int?  StatusparoxTaxMonth { get; set; }
        public int?  StatusparoxTaxYear { get; set; }
        public int?  UptoPortal { get; set; }
        public int?  Statusexp { get; set; }
        public int?  Statuspayback { get; set; }
    }
}
