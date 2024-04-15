namespace API.DTO
{
    public class TmpInsChangeDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? ChangeTypeId { get; set; }
        public string? RefCode { get; set; }
        public string? Code { get; set; }
        public string? TypeName { get; set; }
        public DateTime? ChangeMonth { get; set; }
        public decimal? SalaryOld { get; set; }
        public decimal? SalaryNew { get; set; }
        public bool? IsBhxh { get; set; }
        public bool? IsBhyt { get; set; }
        public bool? IsBhtn { get; set; }
        public bool? IsBnn { get; set; }
    }
}
