namespace API.DTO
{
    public class SysGroupPermissionDTO
    {
        public long? Id { get; set; }
        public long? GroupId { get; set; }
        public long? FunctionId { get; set; }

        public string? PermissionString { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
