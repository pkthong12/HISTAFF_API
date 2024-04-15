namespace API.DTO
{
    public class HuEmployeePapersDTO
    {
        public long? Id { get; set; }
        public long? PaperId { get; set; }
        public long? EmpId { get; set; }
        public long? StatusId { get; set; }
        public DateTime? DateInput { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Url { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
    public class HuEmployeePapersCountDTO
    {
        public long? EmpId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? PaperssCount { get; set;}

    }
}
