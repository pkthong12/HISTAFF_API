using API.Main;

namespace API.DTO
{
    public class ExportDTO
    {
        public long? periodId { get; set; }
        public long? periodAddId { get; set; }
        public long? salObjId { get; set; }
        public List<long>? lstOrg { get; set; }
        public List<long>? lstRankCode { get; set; }
        public long? dateCalId { get; set; }
        public long? phaseId { get; set; }

        public long? employeeId { get; set; }

    }
    public class ImportDTO
    {
        public long? salObj { get; set; }
        public long? periodId { get; set; }
        public long? periodAddId { get; set; }
        public long? dateCalId{ get; set; }
        public long? phaseId { get; set; }
        public long? recordSuccess { get; set; }
        public long? year { get; set; }
        public string base64 { get; set; }

        public List<long>? lstColVal { get; set; }
    }
}
