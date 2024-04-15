using API.Main;

namespace API.DTO
{
    public class CssThemeVarDTO: BaseDTO
    {
        public long? CssThemeId { get; set; }

        public long? CssVarId { get; set; }

        public string? Value { get; set; }

        public string? ThemeCode { get; set; }

        public string? VarName { get; set; }
    }
}
