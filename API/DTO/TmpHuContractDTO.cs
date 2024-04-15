namespace API.DTO
{
    public class TmpHuContractDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? ContractTypeId { get; set; }
        public long? StatusId { get; set; }
        public string? Name { get; set; }
        public string? RefCode { get; set; }
        public string? Code { get; set; }
        public string? ContractNo { get; set; }
        public string? ContractTypeName { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? SignDate { get; set; }
        public decimal? SalBasic { get; set; }
        public decimal? SalTotal { get; set; }
        public decimal? SalPercent { get; set; }
        public string? StatusName { get; set; }
        public string? SignerName { get; set; }
        public string? SignerPosition { get; set; }
    }
}
