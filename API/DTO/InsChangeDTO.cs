using API.Main;

namespace API.DTO
{
    public class InsChangeDTO : BaseDTO
    {
        //public long? Id { get; set; }
        public long? TenantId { get; set; }
        public int? JobOrderNum { get; set; }
        public long? EmployeeId { get; set; }
        public long? ChangeTypeId { get; set; }//loai bien dong ID
        public DateTime? ChangeMonth { get; set; }

        //public decimal? SalaryOld { get; set; }
        //public decimal? SalaryNew { get; set; }

        public double? SalaryOld { get; set; }
        public double? SalaryNew { get; set; }

        public string? Note { get; set; }
        public bool? IsBhxh { get; set; }
        public bool? IsBhtn { get; set; }
        public bool? IsBhyt { get; set; }
        public bool? IsBnn { get; set; }




        //khiemdh them
        //0.thong tin nhan vien
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? IdNo { get; set; }//so cc
        public DateTime? IdDate { get; set; }
        public long? IdPlace { get; set; }
        public string? PlaceName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BirthPlace { get; set; }
        public string? AddressIdentity { get; set; }
        public string? PositionName { get; set; }
        public string? OrgName { get; set; }
        public long? OrgId { get; set; }
        public string? CompName { get; set; }


        //1.thong tin bao hiem
        public string? BhxhNo { get; set; }//so so bhxh
        public long? UnitInsuranceTypeId { get; set; }//đơn vị BH
        public string? UnitInsuranceTypeName { get; set; }
        public string? ChangeTypeName { get; set; } //loai bien dong
        public List<int>? InsuranceType { get; set; } = new List<int>();//loai bao hiem
        public decimal? SalaryBhxhBhytOld { get; set; }//mức đóng bhxh-bhyt cũ
        public decimal? SalaryBhxhBhytNew { get; set; }//------------------ mới
        public decimal? SalaryBhtnOld { get; set; } //mức đóng bhtn cũ
        public decimal? SalaryBhtnNew { get; set; }//------------- mới
        public DateTime? EffectiveDate { get; set; }//ngày hiệu lực
        public DateTime? ExpireDate { get; set; }//hết hiệu lực
        public DateTime? DeclarationPeriod { get; set; }//đợt khai báo
        public DateTime? BhytReimbursementDate { get; set; }//ngày trả thẻ

        //2.thong tin truy thu
        public DateTime? ArrearsFromMonth { get; set; }//truy thu từ tháng
        public DateTime? ArrearsToMonth { get; set; }//truy thu đến tháng
        public decimal? ArBhxhSalaryDifference { get; set; }//mức chênh lệch truy thu bhxh
        public decimal? ArBhytSalaryDifference { get; set; }//mức chênh lệch truy thu bhyt
        public decimal? ArBhtnSalaryDifference { get; set; }//mức chênh lệch truy thu bhtn
        public decimal? ArBhtnldBnnSalaryDifference { get; set; }//mức chênh lệch truy thu bnn

        //3.thong tin thoai thu
        public DateTime? WithdrawalFromMonth { get; set; }//thoái thu từ tháng
        public DateTime? WithdrawalToMonth { get; set; }//thoái thu đến tháng
        public decimal? WdBhxhSalaryDifference { get; set; }//mức thoái thu chênh lệch bhxh
        public decimal? WdBhytSalaryDifference { get; set; }//mức thoái thu chênh lệch bhyt
        public decimal? WdBhtnSalaryDifference { get; set; }//mức thoái thu chênh lệch bhtn
        public decimal? WdBhtnldBnnSalaryDifference { get; set; }//mức thoái thu chênh lệch bnn


        public string? ChangeMonthString { get; set; }//Tháng biến động
        public string? DeclarationPeriodString { get; set; }//đợt khai báo
        public string? ArrearsFromMonthString { get; set; }//truy thu từ tháng
        public string? ArrearsToMonthString { get; set; }//truy thu đến tháng
        public string? WithdrawalFromMonthString { get; set; }//thoái thu từ tháng
        public string? WithdrawalToMonthString { get; set; }//thoái thu đến tháng
        public long? IsTruyThu { get; set; }
    }
}
