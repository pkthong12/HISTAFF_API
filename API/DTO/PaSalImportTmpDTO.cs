namespace API.DTO
{
    public class PaSalImportTmpDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? ElementId { get; set; }
        public long? PeriodId { get; set; }
        public string? RefCode { get; set; }
        public string? EmployeeCode { get; set; }
        public string? Values { get; set; }
        public int? TypeSal { get; set; }
    }
}
