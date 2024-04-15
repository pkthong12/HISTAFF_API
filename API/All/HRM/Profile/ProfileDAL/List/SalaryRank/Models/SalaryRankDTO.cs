using API.Entities;
using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class SalaryRankDTO : Pagings
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? SalaryScaleId { get; set; }
        public string? SalaryScaleName { get; set; }
        public int? Orders { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }

    public class SalaryRankInputDTO
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? SalaryScaleId { get; set; }
        public int? Orders { get; set; }
        public int? LevelStart { get; set; }
        public string Note { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
    }


    public class SalaryRankCountDTO
    {
        public int STT { get; set; }
        public long? Id { get; set; }
        public string Name { get; set; }
        public string ScaleName { get; set; }
        public int ScaleId { get; set; }
        public int Count { get; set; }
        public List<HU_SALARY_LEVEL> lstSalaryLevel { get; set; }
    }
    public class SalaryResultDTO
    {
        public int STT { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public List<SalaryRankR> SalaryRanks { get; set; }
    }
    public class SalaryRankR
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<SalaryLevelDTO> SalaryLevels { get; set; }
    }
    public class SalaryRankStart
    {
        public string Code { get; set; }
        public int LevelStart { get; set; }
    }
}
