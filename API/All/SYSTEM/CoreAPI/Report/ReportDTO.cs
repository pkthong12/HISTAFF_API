namespace API.All.SYSTEM.CoreAPI.Report
{
    public class ParameterValue
    {
        public required string FeildName { get; set; }
        public required string DataType { get; set; }
        public required string DtoName { get; set; }
    }

    public class ReportDTO
    {
        public required string ErCode { get; set; } = null!;
        public required string StoreName { get; set; }
        public long? PeriodId { get; set; }
        public long? EmployeeId { get; set; }
        public string? WorkStatus { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public long[]? OrgIds { get; set; }
        public long? OrgId { get; set; }
        public string? ListOrgIds { get; set; }
        public string? Lang { get; set; }
        public string? EmployeeDeclare { get; set; }
        public string? EmployeeRepresentative { get; set; }
        public DateTime? ReportDate { get; set; }
    }

    public class HeaderTitle
    {
        public required string Cell { get; set; }
        public required string Text { get; set; }
    }

    public class HeaderNoTranslate
    {
        public required string Cell { get; set; }
        public required string Text { get; set; }
    }

    public class RowHeigh
    {
        public required int RowNumber { get; set; }
        public required int Height { get; set; }
    }

    public class ColumnWidth
    {
        public required int ColumnNumber { get; set; }
        public required int Width { get; set; }
    }

    public class HeaderGridTitle
    {
        public required string Cell { get; set; }
        public required string Text { get; set; }
    }

    public class GridContent
    {
        public required string Cell { get; set; }
        public required string Text { get; set; }
    }

    public class FooterTitle
    {
        public required string Cell { get; set; }
        public required string Text { get; set; }
    }

    public class FooterNoTranslate
    {
        public required string Cell { get; set; }
        public required string Text { get; set; }
    }

    public class FontSizeSpecific
    {
        public required string Cell { get; set; }
        public required int Size { get; set; }
    }
}
