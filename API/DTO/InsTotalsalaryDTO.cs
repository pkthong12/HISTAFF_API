using API.Main;

namespace API.DTO
{
    public class InsTotalsalaryDTO : BaseDTO
    {
        public long? InsChangeId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public long? PeriodId { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? BhxhNo { get; set; }
        public string? DeclareDate { get; set; }
        public long? PositionId { get; set; }
        public string? PositionName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        // so tien phai dong nhan vien - cty
        public long? SiEmp { get; set; }
        public long? SiCom { get; set; }
        public long? HiEmp { get; set; }
        public long? HiCom { get; set; }
        public long? UiEmp { get; set; }
        public long? UiCom { get; set; }
        public long? BhtnldBnnCom { get; set; }
        public long? BhtnldBnnEmp { get; set; }
        public long? TotalEmp { get; set; }
        public long? TotalCom { get; set; }

        public long? OldSal { get; set; }
        public long? NewSal { get; set; }
        public long? SiAdjust { get; set; }
        public long? HiAdjust { get; set; }
        public long? UiAdjust { get; set; }
        public long? BhtnldBnnAdjust { get; set; }

        public long? HiAdd { get; set; }
        public long? UiAdd { get; set; }
        // % DONG 
        public decimal? RateSiCom { get; set; }
        public decimal? RateSiEmp { get; set; }
        public decimal? RateHiCom { get; set; }
        public decimal? RateHiEmp { get; set; }
        public decimal? RateUiCom { get; set; }
        public decimal? RateUiEmp { get; set; }
        public decimal? RateBhtnldBnnCom { get; set; }
        public decimal? RateBhtnldBnnEmp { get; set; }

        public long? Status { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
        public long? ASiEmp { get; set; }
        public long? AHiEmp { get; set; }
        public long? AUiEmp { get; set; }
        public long? ASiCom { get; set; }
        public long? AHiCom { get; set; }
        public long? AUiCom { get; set; }
        public long? RSiEmp { get; set; }
        public long? RHiEmp { get; set; }
        public long? RUiEmp { get; set; }
        public long? RSiCom { get; set; }
        public long? RHiCom { get; set; }
        public long? RUiCom { get; set; }
        public long? OSiEmp { get; set; }
        public long? OHiEmp { get; set; }
        public long? OUiEmp { get; set; }
        public long? OSiCom { get; set; }
        public long? OHiCom { get; set; }
        public long? OUiCom { get; set; }
        public long? InsOrgId { get; set; }
        public long? InsArisingTypeId { get; set; }
        public long? Si { get; set; }
        public long? Hi { get; set; }
        public long? Ui { get; set; }
        public long? SiAdd { get; set; }
        public long? ArisingGroupId { get; set; }
        public long? SiSal { get; set; }
        public long? HiSal { get; set; }
        public long? UiSal { get; set; }
        public long? IsDeleted { get; set; }
        public long? SiAdjustCom { get; set; }
        public long? HiAdjustCom { get; set; }
        public long? UiAdjustCom { get; set; }
        public long? BhtnldBnnAdjustCom { get; set; }


        public long? SiSalOld { get; set; }
        public long? HiSalOld { get; set; }
        public long? UiSalOld { get; set; }

        public long? BhtnldBnnAdjustEmp { get; set; }

        public long? BhtnldBnn { get; set; }
        public long? BhtnldSal { get; set; }
        public long? BhtnldSalOld { get; set; }
        public long? ABhtnldBnn { get; set; }
        public long? RBhtnldBnn { get; set; }
        public long? ABhtnldBnnEmp { get; set; }
        public long? ABhtnldBnnCom { get; set; }
        public long? RBhtnldBnnEmp { get; set; }
        public long? RBhtnldBnnCom { get; set; }
    }
}
