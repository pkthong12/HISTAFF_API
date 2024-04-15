namespace API.DTO
{
    public class SysOtherListDTO
    {
        public long? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public long? TypeId { get; set; }
        public int? Orders { get; set; }

        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string? Note { get; set; }
        public string? TypeCode { get; set; }
        public string? TypeName { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? Status { get; set; }

    }
}
