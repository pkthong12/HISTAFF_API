using API.Main;

namespace API.DTO
{
    public class SysMenuDTO: BaseDTO
    {

        public string? Code { get; set; }
        
        public bool? RootOnly { get; set; }

        public int? OrderNumber { get; set; }

        public string? IconFontFamily { get; set; }

        public string? IconClass { get; set; }

        public string? SysMenuServiceMethod { get; set; }

        public string? Url { get; set; }

        public long? Parent { get; set; }

        public long? FunctionId { get; set; }

        public bool? Inactive { get; set; }

        // Extra
        public int? OrderNumberAuto { get; set; }
        public string? FunctionName { get; set; }

        // if Protected, user will only see the item, but no selection allow on CoreOrgTree
        public bool? Protected { get; set; }

    }
}
