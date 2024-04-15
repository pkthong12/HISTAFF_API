
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("REPORT_DATA_STAFF_PROFILE")]
[Keyless]
public partial class REPORT_DATA_STAFF_PROFILE
{
    public string? AVATAR { get; set; }

    public string FULL_NAME { get; set; } = null!;
    public string? ORG_NAME { get; set; }
    public long? ORG_ID { get; set; }

    public string? OTHER_NAME { get; set; }

    public string? GENDER { get; set; }

    public DateTime? BIRTH_DATE { get; set; }

    public string? ID_NO { get; set; }

    public DateTime? ID_DATE { get; set; }

    public string? ID_PLACE_NAME { get; set; }
    public string? NATIONALITY { get; set; }
    public string? WEIGHT{ get; set; }
    public string? HEIGHT{ get; set; }
    public string? PROVINCE { get; set; }
    public string? CURRENT_PROVINCE { get; set; }
    public string? ADDRESS { get; set; }
    public string? CUR_ADDRESS { get; set; }
    public string? TAX_CODE { get; set; }
    public string? MOBILE_PHONE { get; set; }
    public string? WORK_EMAIL { get; set; }
    public string? EMAIL { get; set; }
    public string? BIRTH_PLACE { get; set; }
    public string? PASS_PLACE { get; set; }
    public string? PASS_NO { get; set; }
    public string? VISA_NO { get; set; }
    public string? VISA_PLACE { get; set; }
    public DateTime? PASS_DATE { get; set; }
    public DateTime? VISA_DATE { get; set; }
    public DateTime? VISA_EXPIRE { get; set; }
    public DateTime? PASS_EXPIRE { get; set; }
    public DateTime? WORK_PERMIT_DATE { get; set; }
    public DateTime? WORK_PERMIT_EXPIRE { get; set; }
    public string? WORK_NO { get; set; }
    public string? WORK_PERMIT_PLACE { get; set; }
    public string? DOMICILE { get; set; }
    public DateTime? TAX_CODE_DATE { get; set; }
    public string? BIRTH_REGIS_ADDRESS { get; set; }
    public string? BLOOD_GROUP { get; set; }
    public string? BLOOD_PRESSURE { get; set; }
    public string? HEALTH_TYPE { get; set; }
    public string? LEFT_EYE { get; set; }
    public string? RIGHT_EYE { get; set; }
    public string? HEART { get; set; }
    public string? HEALTH_NOTES { get; set; }
    public string? CARRER { get; set; }
    public string? INSURENCE_NUMBER { get; set; }
    public string? HEALTH_CARE_ADDRESS { get; set; }
    public string? INS_CARD_NUMBER { get; set; }
    public string? FAMILY_MEMBER { get; set; }
    public string? FAMILY_POLICY { get; set; }
    public string? POLITICAL_THEORY { get; set; }
    public string? WORK_STATUS_ID { get; set; }
    public string? TITLE_CONFERRED { get; set; }
    public string? SCHOOL_OF_WORK { get; set; }
    public string? PRISON_NOTE { get; set; }
    public string? FAMILY_DETAIL { get; set; }
    public string? UNIONIST_POSITION { get; set; }
    public string? UNIONIST_ADDRESS { get; set; }
    public string? MEMBER_POSITION { get; set; }
    public string? MEMBER_ADDRESS { get; set; }
    public string? LIVING_CELL { get; set; }
    public string? CARD_NUMBER { get; set; }
    public string? POLITICAL_THEORY_LEVEL { get; set; }
    public string? RESUME_NUMBER { get; set; }
    public string? VATERANS_POSITION { get; set; }
    public string? VATERANS_ADDRESS { get; set; }
    public string? HIGHEST_MILITARY_POSITION { get; set; }
    public string? CURRENT_PARTY_COMMITTEE { get; set; }
    public string? PARTYTIME_PARTY_COMMITTEE { get; set; }
    public string? HOUSEHOLD_NUMBER { get; set; }
    public string? HOUSEHOLD_CODE { get; set; }
    public string? MOBILE_PHONE_LAND { get; set; }
    public string? MAIN_INCOME { get; set; }
    public string? OTHER_SOURCES { get; set; }
    public string? LAND_GRANTED { get; set; }
    public string? TAX_GRANTED_HOUSE { get; set; }
    public string? TOTAL_AREA { get; set; }
    public string? SELF_PURCHASE_LAND { get; set; }
    public string? SELF_BUILD_HOUSE { get; set; }
    public string? TOTAL_APP_AREA { get; set; }
    public string? LAND_FOR_PRODUCTION { get; set; }
    public string? ADDITIONAL_INFOMATION { get; set; }
    public string? YOUTH_SAVE_NATION_POSITION { get; set; }
    public string? YOUTH_SAVE_NATION_ADDRESS { get; set; }
    public string? YELLOW_FLAG { get; set; }
    public string? RELATIONS { get; set; }
    public string? YOUTH_GROUP_POSITION { get; set; }
    public string? YOUTH_GROUP_ADDRESS { get; set; }
    public string? ITIME_ID { get; set; }
    public string? DISTRICT { get; set; }
    public string? CODE { get; set; }
    public string? CUR_DISTRICT { get; set; }
    public string? MARITAL_STATUS { get; set; }
    public string? WARD { get; set; }
    public string? CUR_WARD { get; set; }
    public string? BANK_NAME_1 { get; set; }
    public string? BANK_BRANCH_NAME_1 { get; set; }
    public string? BANK_NO { get; set; }
    public string? QUALIFICATION_1 { get; set; }
    public string? TRAINING_FORM_1 { get; set; }
    public string? LEARNING_LEVEL_1 { get; set; }
    public string? NATIVE { get; set; }
    public string? RELIGION { get; set; }
    public string? EDUCATION_LEVEL { get; set; }
    public string? QUALIFICATION_2 { get; set; }
    public string? QUALIFICATION_3 { get; set; }
    public string? SCHOOL { get; set; }
    public string? SCHOOL_2 { get; set; }
    public string? SCHOOL_3 { get; set; }
    public string? TRAINING_FORM_2 { get; set; }
    public string? TRAINING_FORM_3 { get; set; }
    public string? LANGUAGE_1 { get; set; }
    public string? LANGUAGE_2 { get; set; }
    public string? LANGUAGE_3 { get; set; }
    public string? LANGUAGE_LEVEL_1 { get; set; }
    public string? LANGUAGE_LEVEL_2 { get; set; }
    public string? LANGUAGE_LEVEL_3 { get; set; }
    public string? PRESENTER { get; set; }
    public string? PRESENTER_PHONE_NUMBER { get; set; }
    public string? PRESENTER_ADDRESS { get; set; }
    public string? TAX_CODE_ADDRESS { get; set; }
    public string? BANK_NAME_2 { get; set; }
    public string? BANK_BRANCH_NAME_2 { get; set; }
    public string? BANK_NO_2 { get; set; }
    public string? LICENSE { get; set; }
    public string? COMPUTER_SKILL { get; set; }
    public string? EMPLOYEE_OBJECT { get; set; }
    public string? INS_WHEREHEALTH { get; set; }
    public DateTime? MEMBER_OFFICAL_DATE { get; set; }
    public DateTime? YOUTH_GROUP_DATE { get; set; }
    public DateTime? YOUTH_SAVE_NATION_DATE { get; set; }
    public DateTime? EXAMINATION_DATE { get; set; }
    public DateTime? UNIONIST_DATE { get; set; }
    public DateTime? VATERANS_MEMBER_DATE { get; set; }
    public DateTime? MEMBER_DATE { get; set; }
    public DateTime? ENLISTMENT_DATE { get; set; }
    public DateTime? DISCHARGE_DATE { get; set; }
}
