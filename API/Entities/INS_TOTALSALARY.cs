
using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities

{
    [Table("INS_TOTALSALARY")]
    public class INS_TOTALSALARY : BASE_ENTITY
    {
        public int? YEAR { get; set; }
        public int? MONTH { get; set; }
        public long? PERIOD_ID { get; set; }
        public long? INS_CHANGE_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public long? ORG_ID { get; set; }
        public long? SI_EMP { get; set; } //BHXH: NHAN VIEN DONG
        public long? SI_COM { get; set; } //BHXH: CTY DONG
        public long? HI_EMP { get; set; } //BHXH: NHAN VIEN DONG
        public long? HI_COM { get; set; }//BHXH: NHAN VIEN DONG
        public long? UI_EMP { get; set; }//BHXH: NHAN VIEN DONG
        public long? UI_COM { get; set; }//BHXH: NHAN VIEN DONG
        public long? BHTNLD_BNN_COM { get; set; }
        public long? BHTNLD_BNN_EMP { get; set; }
        public long? OLD_SAL { get; set; }
        public long? NEW_SAL { get; set; }
        //dieu chinh nv
        public long? SI_ADJUST { get; set; }
        public long? HI_ADJUST { get; set; }
        public long? UI_ADJUST { get; set; }
        public long? BHTNLD_BNN_ADJUST_EMP { get; set; }

        public long? HI_ADD { get; set; }
        public long? UI_ADD { get; set; }
        // % DONG 
        public decimal? RATE_SI_COM { get; set; } 
        public decimal? RATE_SI_EMP { get; set; }
        public decimal? RATE_HI_COM { get; set; }
        public decimal? RATE_HI_EMP { get; set; }
        public decimal? RATE_UI_COM { get; set; }
        public decimal? RATE_UI_EMP { get; set; }
        public decimal? RATE_BHTNLD_BNN_COM { get; set; }
        public decimal? RATE_BHTNLD_BNN_EMP { get; set; }

        public long? STATUS { get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
        public long? A_SI_EMP { get; set; }
        public long? A_HI_EMP { get; set; }
        public long? A_UI_EMP { get; set; }
        public long? A_SI_COM { get; set; }
        public long? A_HI_COM { get; set; }
        public long? A_UI_COM { get; set; }
        public long? R_SI_EMP { get; set; }
        public long? R_HI_EMP { get; set; }
        public long? R_UI_EMP { get; set; }
        public long? R_SI_COM { get; set; }
        public long? R_HI_COM { get; set; }
        public long? R_UI_COM { get; set; }
        public long? O_SI_EMP { get; set; }
        public long? O_HI_EMP { get; set; }
        public long? O_UI_EMP { get; set; }
        public long? O_SI_COM { get; set; }
        public long? O_HI_COM { get; set; }
        public long? O_UI_COM { get; set; }
        public long? INS_ORG_ID { get; set; } //ID DON VI BAO HIEM
        public long? INS_ARISING_TYPE_ID { get; set; }
        public long? SI { get; set; }
        public long? HI { get; set; }
        public long? UI { get; set; }
        public long? SI_ADD { get; set; }
        public long? ARISING_GROUP_ID { get; set; } //NHOM BIEN DONG( 1 TANG, 2 GIAM, 3 DIEU CHINH)
        public long? SI_SAL { get; set; }
        public long? HI_SAL { get; set; }
        public long? UI_SAL { get; set; }
        public long? IS_DELETED { get; set; }
        //dieu chinh cty
        public long? SI_ADJUST_COM { get; set; }
        public long? HI_ADJUST_COM { get; set; }
        public long? UI_ADJUST_COM { get; set; }
        public long? BHTNLD_BNN_ADJUST_COM { get; set; }

        public long? SI_SAL_OLD { get; set; }
        public long? HI_SAL_OLD { get; set; }
        public long? UI_SAL_OLD { get; set; }
        

        public long? BHTNLD_BNN_ADJUST { get; set; }
        public long? BHTNLD_BNN { get; set; }
        public long? BHTNLD_SAL { get; set; }
        public long? BHTNLD_SAL_OLD { get; set; }
        public long? A_BHTNLD_BNN { get; set; }
        public long? R_BHTNLD_BNN { get; set; }
        public long? A_BHTNLD_BNN_EMP { get; set; }
        public long? A_BHTNLD_BNN_COM { get; set; }
        public long? R_BHTNLD_BNN_EMP { get; set; }
        public long? R_BHTNLD_BNN_COM { get; set; }
    }
}
