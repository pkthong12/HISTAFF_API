namespace API.DTO
{
    public class ExportExelRequest
    {
        public List<CoreTableColumnDTO>? Headers { get; set; }
        public object? ListData { get; set; }
    }
}
