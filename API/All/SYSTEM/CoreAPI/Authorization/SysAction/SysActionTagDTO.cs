namespace API.All.SYSTEM.CoreAPI.Authorization.SysAction
{
    public class SysActionTagDTO
    {
        public required long Id { get; set; }
        public required string Text { get; set; }
        public bool? Enabled { get; set; }
    }
}
