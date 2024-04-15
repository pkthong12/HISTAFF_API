using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_WELFARE_MNG")]

public class HU_WELFARE_MNG: BASE_ENTITY
{
    public long? PROFILE_ID { get; set; }
    public long? WELFARE_ID { get; set; }   // ma phuc loi

    public long? EMPLOYEE_ID { get; set; }  // ma nhan vien 

    public DateTime? EFFECT_DATE { get; set; }  // ngay hieu luc

    public DateTime? EXPIRE_DATE { get; set; }  // ngay het hieu luc

    public string? NOTE { get; set; }   // ghi chu

    public decimal? MONEY { get; set; } // so tien phuc loi

    public long? PERIOD_ID { get; set; }    // ky luong

    public DateTime? PAY_DATE { get; set; }     // ngay tra

    public string? DECISION_CODE { get; set; }  // so quyet dinh

    public bool? IS_TRANSFER { get; set; }     // chuyen khoan

    public bool? IS_CASH { get; set; }    // chi ngoai

}
