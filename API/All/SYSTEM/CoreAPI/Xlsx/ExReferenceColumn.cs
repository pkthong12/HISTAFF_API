namespace API.All.SYSTEM.CoreAPI.Xlsx
{
    public class ExReferenceColumn
    {
        public string Text { get; set; } = null!;
        public long Value { get; set; }
    }

    public class ExReferenceColumnReverse
    {
        public long Value { get; set; }
        public string Text { get; set; } = null!;
    }
}
