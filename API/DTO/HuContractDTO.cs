namespace API.DTO
{
    public class HuContractDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? ContractTypeId { get; set; }
        public long? SignId { get; set; }
        public long? StatusId { get; set; }
        public long? WorkingId { get; set; }
        public string? ContractNo { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? SignerName { get; set; }
        public string? SignerPosition { get; set; }
        public decimal? SalBasic { get; set; }
        public decimal? SalPercent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? SignDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsReceive { get; set; }

    }

    public class HuContractImportDTO: HuContractDTO
    {
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? OrgName { get; set; }
        public long? OrgId { get; set; }
        public string? StatusName { get; set; }
        public string? ContractTypeName { get; set; }
        public string? PositionName { get; set; }
        public string? XlsxUserId { get; set; }
        public string? XlsxExCode { get; set; }
        public DateTime? XlsxInsertOn { get; set; }
        public long? XlsxSession { get; set; }
        public string? XlsxFileName { get; set; }
        public int? XlsxRow { get; set; }
    }

}
