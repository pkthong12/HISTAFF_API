namespace API.Main
{
    public class BaseDTO
    {
        public long? Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedByUsername { get; set; }
        public string? UpdatedByUsername { get; set; }

        public SysMutationLogBeforeAfterRequest? SysMutationLogBeforeAfterRequest { get; set; }
        public List<string>? ActualFormDeclaredFields { get; set; }

    }
}
