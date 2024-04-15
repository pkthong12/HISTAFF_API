using API.Main;

namespace API.DTO
{
    public class HuWelfareMngDTO: BaseDTO
    {
        public long? WelfareId { get; set; }   // ma phuc loi

        public string? WelfareName { get; set; }

        public long? EmployeeId { get; set; }  // ma nhan vien 

        public long? WorkStatusId { get; set; }

        public long? OrgId { get; set; }

        public string? EmployeeCode { get; set; }

        public string? EmployeeName { get; set; }

        public string? PositionName { get; set; }

        public string? DepartmentName { get; set; }

        public DateTime? EffectDate { get; set; }  // ngay hieu luc

        public DateTime? ExpireDate { get; set; }  // ngay het hieu luc

        public string? Note { get; set; }   // ghi chu

        public decimal? Money { get; set; } // so tien phuc loi

        public long? PeriodId { get; set; }    // ky luong

        public string? PeriodName { get; set; }

        public DateTime? PayDate { get; set; }     // ngay tra

        public string? DecisionCode { get; set; }  // so quyet dinh

        public bool? IsTransfer { get; set; }     // chuyen khoan

        public bool? IsCash { get; set; }    // chi ngoai           
        public int? JobOrderNum { get; set; }
    }
}
