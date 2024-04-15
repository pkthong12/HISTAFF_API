using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_EMPLOYEE_TMP")]
public partial class HU_EMPLOYEE_TMP
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string? REF_CODE { get; set; }

    public string FIRST_NAME { get; set; } = null!;

    public string LAST_NAME { get; set; } = null!;

    public string? FULLNAME { get; set; }

    public string? ORG_NAME { get; set; }

    public int? ORG_ID { get; set; }

    public string? POSITION { get; set; }

    public int? POS_ID { get; set; }

    public string? RESIDENT { get; set; }

    public int? RESIDENT_ID { get; set; }

    public string? GENDER { get; set; }

    public int? GENDER_ID { get; set; }

    public string? BIRTH_DATE { get; set; }

    public DateTime? BIRTH_DATE_INPUT { get; set; }

    public string? ID_NO { get; set; }

    public DateTime? ID_DATE_INPUT { get; set; }

    public string? ID_PLACE { get; set; }

    public string? RELIGION { get; set; }

    public int? RELIGION_ID { get; set; }

    public string? NATIVE { get; set; }

    public int? NATIVE_ID { get; set; }

    public string? NATIONALITY { get; set; }

    public int? NATIONALITY_ID { get; set; }

    public string? ADDRESS { get; set; }

    public string? BIRTH_PLACE { get; set; }

    public string? PROVINCE { get; set; }

    public int? PROVINCE_ID { get; set; }

    public string? DISTRICT { get; set; }

    public int? DISTRICT_ID { get; set; }

    public string? WARD { get; set; }

    public int? WARD_ID { get; set; }

    public string? CUR_ADDRESS { get; set; }

    public string? CUR_PROVINCE { get; set; }

    public int? CUR_PROVINCE_ID { get; set; }

    public string? CUR_DISTRICT { get; set; }

    public int? CUR_DISTRICT_ID { get; set; }

    public string? CUR_WARD { get; set; }

    public int? CUR_WARD_ID { get; set; }

    public string? ITIME_ID { get; set; }

    public string? TAX_CODE { get; set; }

    public string? CONTACT_PER { get; set; }

    public string? CONTACT_PER_PHONE { get; set; }

    public string? MOBILE_PHONE { get; set; }

    public string? WORK_EMAIL { get; set; }

    public string? EMAIL { get; set; }

    public string? MARITAL_STATUS { get; set; }

    public int? MARITAL_STATUS_ID { get; set; }

    public string? PASS_NO { get; set; }

    public DateTime? PASS_DATE_INPUT { get; set; }

    public DateTime? PASS_EXPIRE_INPUT { get; set; }

    public string? PASS_PLACE { get; set; }

    public string? VISA_NO { get; set; }

    public DateTime? VISA_DATE_INPUT { get; set; }

    public DateTime? VISA_EXPIRE_INPUT { get; set; }

    public string? VISA_PLACE { get; set; }

    public string? WORK_PERMIT { get; set; }

    public DateTime? WORK_PERMIT_DATE_INPUT { get; set; }

    public DateTime? WORK_PERMIT_EXPIRE_INPUT { get; set; }

    public string? WORK_PERMIT_PLACE { get; set; }

    public string? BANK_NAME { get; set; }

    public string? BANK_BRANCH { get; set; }

    public string? BANK_NO { get; set; }

    public int? BANK_ID { get; set; }

    public string? SCHOOL_ID { get; set; }

    public int? EMP_ID { get; set; }

    public string? QUALIFICATION_ID { get; set; }

    public string? TRAINING_FORM { get; set; }

    public int? TRAINING_FORM_ID { get; set; }

    public string? LEARNING_LEVEL { get; set; }

    public int? LEARNING_LEVEL_ID { get; set; }

    public string? LANGUAGE { get; set; }
}
