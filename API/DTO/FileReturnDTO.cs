namespace API.DTO
{
    public class FileReturnDTO
    {
        public byte[]? Bytes { get; set; }
        public string? ContentType { get; set; }
        public string? FileName { get; set; }
        public string? Base64 { get; set; }
        public string? RefId { get; set; }
    }
}
