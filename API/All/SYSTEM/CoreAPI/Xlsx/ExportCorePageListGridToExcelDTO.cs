using System.Dynamic;

namespace API.All.SYSTEM.CoreAPI.Xlsx
{
    public class ExportCorePageListGridToExcelDTO
    {
        public required List<CoreTableColumnItem> Columns { get; set; }
        public required List<ExpandoObject> Data { get; set; }
    }

    public class CoreTableColumnItem
    {
        public required string Caption { get; set; }
        public required string Field { get; set; }
        public string? Type { get; set; }
        public string? Pipe { get; set; }
        public string? Align { get; set; }
        public int Width { get; set; }
        public bool? Hidden { get; set; }
    }
}