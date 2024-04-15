using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_EMPLOYEE_EDIT")]
public partial class HU_EMPLOYEE_EDIT
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? ID { get; set; }

    public long EMPLOYEE_ID { get; set; }

    public string? CODE { get; set; }

    public string? FIRST_NAME { get; set; }

    public string? LAST_NAME { get; set; }

    public string? FULLNAME { get; set; }

    public long? ORG_ID { get; set; }

    public long? CONTRACT_ID { get; set; }

    public int? CONTRACT_TYPE_ID { get; set; }

    public long? POSITION_ID { get; set; }

    public long? DIRECT_MANAGER { get; set; }

    public long? LAST_WORKING_ID { get; set; }

    public int? DIRECT_MANAGER_ID { get; set; }

    public int? GENDER_ID { get; set; }

    public string? ID_NO { get; set; }

    public string? ID_PLACE { get; set; }

    public int? RELIGION_ID { get; set; }

    public int? NATIVE_ID { get; set; }

    public int? NATIONALITY_ID { get; set; }

    public string? ADDRESS { get; set; }

    public string? BIRTH_PLACE { get; set; }

    public int? WORK_STATUS_ID { get; set; }

    public long? PROVINCE_ID { get; set; }

    public long? DISTRICT_ID { get; set; }

    public long? WARD_ID { get; set; }

    public DateTime? TER_EFFECT_DATE { get; set; }

    public string? ITIME_ID { get; set; }

    public DateTime? JOIN_DATE { get; set; }

    public long? SALARY_TYPE_ID { get; set; }

    public string? TAX_CODE { get; set; }

    public string? MOBILE_PHONE { get; set; }

    public string? WORK_EMAIL { get; set; }

    public string? EMAIL { get; set; }

    public int? MARITAL_STATUS_ID { get; set; }

    public string? PASS_NO { get; set; }

    public DateTime? PASS_DATE { get; set; }

    public DateTime? PASS_EXPIRE { get; set; }

    public string? PASS_PLACE { get; set; }

    public string? VISA_NO { get; set; }

    public DateTime? VISA_DATE { get; set; }

    public DateTime? VISA_EXPIRE { get; set; }

    public string? VISA_PLACE { get; set; }

    public int? SENIORITY { get; set; }

    public long? STATUS { get; set; }

    public DateTime? BIRTH_DATE { get; set; }

    public string? WORK_PERMIT { get; set; }

    public DateTime? WORK_PERMIT_DATE { get; set; }

    public DateTime? WORK_PERMIT_EXPIRE { get; set; }

    public string? WORK_PERMIT_PLACE { get; set; }

    public string? WORK_NO { get; set; }

    public DateTime? WORK_DATE { get; set; }

    public string? WORK_SCOPE { get; set; }

    public string? WORK_PLACE { get; set; }

    public string? CONTACT_PER { get; set; }

    public string? CONTACT_PER_PHONE { get; set; }

    public long? BANK_ID { get; set; }

    public string? BANK_BRANCH { get; set; }

    public string? BANK_NO { get; set; }

    public string? SCHOOL_ID { get; set; }

    public string? SCHOOLNAME { get; set; }

    public string? TRAININGFORMNAME { get; set; }

    public string? LEARNINGLEVELNAME { get; set; }

    public string? QUALIFICATION_ID { get; set; }

    public int? QUALIFICATIONID { get; set; }

    public string? LANGUAGE_MARK { get; set; }

    public int? TRAINING_FORM_ID { get; set; }

    public int? LEARNING_LEVEL_ID { get; set; }

    public string? LANGUAGE { get; set; }

    public int? RESIDENT_ID { get; set; }

    public int? SAL_TOTAL { get; set; }

    public DateTime? ID_DATE { get; set; }

    public string? CUR_ADDRESS { get; set; }

    public int? CUR_WARD_ID { get; set; }

    public int? CUR_DISTRICT_ID { get; set; }

    public int? CUR_PROVINCE_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public int? STAFF_RANK_ID { get; set; }
    public bool? IS_SEND_PORTAL { get; set; }
    public bool? IS_APPROVED_PORTAL { get; set; }

    public virtual HU_CONTRACT? CONTRACT { get; set; }

    public virtual HU_EMPLOYEE EMPLOYEE { get; set; } = null!;

    public virtual HU_ORGANIZATION? ORG { get; set; }

    public virtual HU_POSITION? POSITION { get; set; }
}
