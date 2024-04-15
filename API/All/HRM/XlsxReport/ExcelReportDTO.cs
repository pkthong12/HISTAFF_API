using API.All.SYSTEM.CoreAPI.Report;

namespace API.All.HRM.XlsxReport
{
    public class ExcelReportDTO
    {

        public required string ClassName { get; set; }
        public required string ErCode { get; set; } // REPORT01
        
        public required int HeaderRow { get; set; } // 7

        /* Thu tu can trung voi cac cot cua data set */
        public float? Size { get; set; }
        public string? FontFamily { get; set; } 
        public List<string>? FontBolds { get; set; }
        public List<string>? FontItalic { get; set; }
        public string? RangeAutoFit { get; set; }
        public string? BorderHeader { get; set; }
        public List<string>? UpperCase { get; set; }
        public List<string>? LowerCase { get; set; }
        public List<string>? MegreCellRange { get; set; }
        public List<string>? HorizontalAlignmentCenter { get; set; }
        public List<string>? HorizontalAlignmentLeft { get; set; }
        public List<string>? VerticalAlignmentCenter { get; set; }
        public List<string>? WrapText { get; set; }
        public List<HeaderTitle>? HeaderTitles { get; set; }
        public List<HeaderNoTranslate>? HeaderNoTranslates { get; set; }
        public List<GridContent>? GridContents { get; set; }
        public List<FooterTitle>? FooterTitles { get; set; }
        public List<FooterNoTranslate>? FooterNoTranslates { get; set; }
        public required List<ParameterValue> ParameterValues { get; set; }
        public List<RowHeigh>? RowHeighs { get; set; }
        public List<ColumnWidth>? ColumnWidths { get; set; }
        public List<HeaderGridTitle>? HeaderGridTitles { get; set; }
        public List<FontSizeSpecific>? FontSizeSpecifics { get; set; }
    }
}
