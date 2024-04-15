namespace API.DTO
{
    public class AtTimesheetFormulaDTO
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? ColName { get; set; }
        public string? Formula { get; set; }
        public string? FormulaName { get; set; }
        public int? Orders { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
