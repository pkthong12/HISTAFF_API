using API.Main;

namespace API.DTO
{
    public class InsSpecifiedObjectsDTO : BaseDTO
    {
        public DateTime? EffectiveDate { get; set; }
        public string? EffectiveDateString { get; set; }

        public int? ChangeDay { get; set; }

        public decimal? SiHi { get; set; }
        
        public decimal? Ui { get; set; }

        public decimal? SiCom { get; set; }

        public decimal? SiEmp { get; set; }

        public decimal? HiCom { get; set; }

        public decimal? HiEmp { get; set; }

        public decimal? UiCom { get; set; }

        public decimal? UiEmp { get; set; }

        public decimal? AiOaiCom { get; set; }

        public decimal? AiOaiEmp { get; set; }

        public int? RetireMale { get; set; }

        public int? RetireFemale { get; set; }
    }
}
