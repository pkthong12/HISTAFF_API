using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_EMPLOYEE_CV_EDIT")]

    public class HU_EMPLOYEE_CV_EDIT : BASE_ENTITY
    {
        public string? AVATAR { get; set; }

        public string? FIRST_NAME { get; set; } = null!;

        public string? LAST_NAME { get; set; } = null!;

        public string? FULL_NAME { get; set; } = null!;
        public string? OTHER_NAME { get; set; }

        public long? GENDER_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }

        public DateTime? BIRTH_DATE { get; set; }

        public string? ID_NO { get; set; }

        public DateTime? ID_DATE { get; set; }

        public long? ID_PLACE { get; set; }

        public long? RELIGION_ID { get; set; }

        public long? NATIVE_ID { get; set; }

        public long? NATIONALITY_ID { get; set; }

        public string? ADDRESS { get; set; }

        public string? BIRTH_PLACE { get; set; }
        public string? CARRER { get; set; }
        public string? INSURENCE_NUMBER { get; set; }

        public string? HEALTH_CARE_ADDRESS { get; set; }
        public string? INS_CARD_NUMBER { get; set; }
        public long? PROVINCE_ID { get; set; }

        public long? DISTRICT_ID { get; set; }

        public long? WARD_ID { get; set; }

        public string? CUR_ADDRESS { get; set; }

        public long? CUR_PROVINCE_ID { get; set; }

        public long? CUR_DISTRICT_ID { get; set; }

        public long? CUR_WARD_ID { get; set; }

        public string? TAX_CODE { get; set; }

        public string? MOBILE_PHONE { get; set; }
        public string? MOBILE_PHONE_LAND { get; set; }
        public bool? IS_HOST { get; set; }

        public string? WORK_EMAIL { get; set; }

        public string? EMAIL { get; set; }

        public long? MARITAL_STATUS_ID { get; set; }

        public string? PASS_NO { get; set; }
        public string? PRISON_NOTE { get; set; }
        public string? FAMILY_DETAIL { get; set; }

        public DateTime? PASS_DATE { get; set; }

        public DateTime? PASS_EXPIRE { get; set; }

        public string? PASS_PLACE { get; set; }

        public string? VISA_NO { get; set; }
        public bool? IS_UNIONIST { get; set; }
        public string? UNIONIST_POSITION { get; set; }
        public DateTime? UNIONIST_DATE { get; set; }
        public DateTime? YOUTH_SAVE_NATION_DATE { get; set; }
        public string? UNIONIST_ADDRESS { get; set; }
        public string? YOUTH_SAVE_NATION_POSITION { get; set; }
        public string? YOUTH_SAVE_NATION_ADDRESS { get; set; }
        public bool? IS_JOIN_YOUTH_GROUP { get; set; }
        public bool? IS_MEMBER { get; set; }
        public string? MEMBER_POSITION { get; set; }
        public string? MEMBER_ADDRESS { get; set; }
        public string? LIVING_CELL { get; set; }
        public string? CARD_NUMBER { get; set; }
        public string? POLITICAL_THEORY_LEVEL { get; set; }
        public string? RESUME_NUMBER { get; set; }
        public DateTime? VATERANS_MEMBER_DATE { get; set; }
        public string? VATERANS_POSITION { get; set; }
        public string? VATERANS_ADDRESS { get; set; }
        public string? YOUTH_GROUP_POSITION { get; set; }
        public string? YOUTH_GROUP_ADDRESS { get; set; }
        public DateTime? YOUTH_GROUP_DATE { get; set; }
        public string? HIGHEST_MILITARY_POSITION { get; set; }
        public string? CURRENT_PARTY_COMMITTEE { get; set; }
        public string? PARTYTIME_PARTY_COMMITTEE { get; set; }
        public DateTime? ENLISTMENT_DATE { get; set; }
        public DateTime? DISCHARGE_DATE { get; set; }
        public DateTime? MEMBER_DATE { get; set; }
        public DateTime? MEMBER_OFFICAL_DATE { get; set; }

        public DateTime? VISA_DATE { get; set; }

        public DateTime? VISA_EXPIRE { get; set; }

        public string? VISA_PLACE { get; set; }

        public string? WORK_PERMIT { get; set; }

        public DateTime? WORK_PERMIT_DATE { get; set; }

        public DateTime? WORK_PERMIT_EXPIRE { get; set; }

        public string? WORK_PERMIT_PLACE { get; set; }

        public string? WORK_NO { get; set; }

        public string? WORK_PLACE { get; set; }

        public string? WORK_SCOPE { get; set; }

        public string? CONTACT_PER { get; set; }

        public string? CONTACT_PER_PHONE { get; set; }
        public string? HOUSEHOLD_NUMBER { get; set; }
        public string? HOUSEHOLD_CODE { get; set; }

        public long? BANK_ID { get; set; }
        public long? BANK_ID_2 { get; set; }
        public long? BANK_BRANCH_ID { get; set; }
        public long? BANK_BRANCH_ID_2 { get; set; }

        public string? BANK_BRANCH { get; set; }
        public string? BANK_NO { get; set; }
        public string? BANK_NO_2 { get; set; }

        public long? SCHOOL_ID { get; set; }
        public long? SCHOOL_ID_2 { get; set; }
        public long? SCHOOL_ID_3 { get; set; }

        public string? QUALIFICATION_ID { get; set; }
        public long? QUALIFICATIONID_2 { get; set; }
        public long? QUALIFICATIONID_3 { get; set; }

        public long? QUALIFICATIONID { get; set; }
        public long? EDUCATION_LEVEL_ID { get; set; }

        public long? TRAINING_FORM_ID { get; set; }
        public long? TRAINING_FORM_ID_2 { get; set; }
        public long? TRAINING_FORM_ID_3 { get; set; }

        public long? LEARNING_LEVEL_ID { get; set; }

        public string? LANGUAGE { get; set; }

        public string? LANGUAGE_MARK { get; set; }

        public string? IMAGE { get; set; }
        public string? HEIGHT { get; set; }
        public string? WEIGHT { get; set; }
        public string? DOMICILE { get; set; }
        public string? BIRTH_REGIS_ADDRESS { get; set; }
        public string? BLOOD_PRESSURE { get; set; }
        public string? HEALTH_TYPE { get; set; }
        public string? LEFT_EYE { get; set; }
        public string? RIGHT_EYE { get; set; }
        public long? TAX_CODE_ADDRESS { get; set; }
        public string? HEART { get; set; }
        public string? HEALTH_NOTES { get; set; }
        //public string? INSURENCE_AREA { get; set; }
        public string? BLOOD_GROUP { get; set; }
        public string? FAMILY_MEMBER { get; set; }
        public string? FAMILY_POLICY { get; set; }
        public string? VETERANS { get; set; }
        public string? POLITICAL_THEORY { get; set; }
        public string? CARRER_BEFORE_RECUITMENT { get; set; }
        public string? TITLE_CONFERRED { get; set; }
        public string? SCHOOL_OF_WORK { get; set; }
        public string? COMPUTER_SKILL { get; set; }
        public string? LICENSE { get; set; }
        public long? LANGUAGE_ID { get; set; }
        public long? LANGUAGE_ID_2 { get; set; }
        public long? LANGUAGE_ID_3 { get; set; }
        public long? LANGUAGE_LEVEL_ID { get; set; }
        public long? LANGUAGE_LEVEL_ID_2 { get; set; }
        public long? LANGUAGE_LEVEL_ID_3 { get; set; }
        public string? PRESENTER { get; set; }
        public string? PRESENTER_PHONE_NUMBER { get; set; }
        public string? PRESENTER_ADDRESS { get; set; }
        public DateTime? TAX_CODE_DATE { get; set; }
        public DateTime? EXAMINATION_DATE { get; set; }
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
        public long? GOVERMENT_MANAGEMENT_ID { get; set; }
        public string? YELLOW_FLAG { get; set; }
        public string? RELATIONS { get; set; }
        public string? ITIME_ID { get; set; }
        public bool? IS_SEND_PORTAL { get; set; }
        public bool? IS_APPROVED_PORTAL { get; set; }
        public bool? IS_APPROVED_CV { get; set; }
        public bool? IS_APPROVED_CONTACT { get; set; }
        public bool? IS_APPROVED_ADDITIONAL_INFO { get; set; }
        public bool? IS_APPROVED_EDUCATION { get; set; }
        public bool? IS_APPROVED_BANK_INFO { get; set; }

        public string? MODEL_CHANGE { get; set; }
        public long? HU_EMPLOYEE_CV_ID { get; set; }
        public bool? IS_SEND_PORTAL_CV { get; set; }
        public bool? IS_SEND_PORTAL_ADDITIONAL_INFO { get; set; }
        public bool? IS_SEND_PORTAL_BANK_INFO { get; set; }
        public bool? IS_SEND_PORTAL_EDUCATION { get; set; }
        public bool? IS_SEND_PORTAL_CONTACT { get; set; }
        public long? STATUS_APPROVED_CV_ID { get; set; }
        public long? STATUS_APPROVED_EDUCATION_ID { get; set; }
        public long? STATUS_APPROVED_BANK_ID { get; set; }
        public long? STATUS_ADDINATIONAL_INFO_ID { get; set; }
        public long? STATUS_APPROVED_CONTACT_ID { get; set; }
        public bool? IS_APPROVED_INSUARENCE_INFO { get; set; }
        public long? STATUS_APPOVED_INSUARENCE_INFO_ID { get; set; }
        public long? INS_WHEREHEALTH_ID { get; set; }
        public bool? IS_SAVE_CV { get; set; }
        public bool? IS_SAVE_CONTACT { get; set; }
        public bool? IS_SAVE_ADDITIONAL_INFO { get; set; }
        public bool? IS_SAVE_BANK_INFO { get; set; }
        public bool? IS_SAVE_EDUCATION { get; set; }
        public bool? IS_SAVE_INSURENCE { get; set; }
        public bool? IS_SAVE_PORTAL { get; set; }
        public string? REASON_DISCARD {  get; set; }
        public long? IDENTITY_ADDRESS { get; set; }
        public DateTime? ID_EXPIRE_DATE { get; set; }
        public string? REASON { get; set; }
        public long? COMPUTER_SKILL_ID { get; set; }
        public long? LICENSE_ID { get; set; }
    }
}
