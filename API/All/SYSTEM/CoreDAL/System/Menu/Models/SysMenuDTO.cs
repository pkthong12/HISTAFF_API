namespace API.All.SYSTEM.CoreDAL.System.Menu.Models
{
    public class SysMenuDTO
    {
        public long? id { get; set; }

        public string? code { get; set; }

        public string? title { get; set; }

        public string? titleForeignLanguage { get; set; }

        public string? iconFontFamily { get; set; }

        public string? iconClass { get; set; }

        public string? url { get; set; }

        public long? parent { get; set; }

        public int? sysFunction { get; set; }

    }
}
