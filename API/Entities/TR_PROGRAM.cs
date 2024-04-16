using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("TR_PROGRAM")]
    public class TR_PROGRAM :BASE_ENTITY
    {
        public string? NAME { get; set; }
        public long? ORG_ID { get; set; }
        public int? YEAR { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public long? TR_COURSE_ID { get; set; }
        public long? DURATION { get; set; }
        public long? TR_DURATION_UNIT_ID { get; set; }
        public long? DURATION_STUDY_ID { get; set; }
        public long? DURATION_HC { get; set; }
        public long? DURATION_OT { get; set; }
        public bool? IS_REIMBURSE { get; set; }
        public long? TR_LANGUAGE_ID { get; set; }
        public string? VENUE { get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
        public string? TITLES { get; set; }
        public string? DEPARTMENTS { get; set; }
        public string? CENTERS { get; set; }
        public string? LECTURES { get; set; }
        public long? TR_REQUEST_ID { get; set; }
        public long? TR_PLAN_ID { get; set; }
        public long? STUDENT_NUMBER { get; set; }
        public long? COST_STUDENT { get; set; }
        public long? TR_UNIT_ID { get; set; }
        public bool? IS_RETEST { get; set; }
        public long? COST_TOTAL_US { get; set; }
        public long? COST_STUDENT_US { get; set; }
        public long? COST_TOTAL { get; set; }
        public long? TRAIN_FORM_ID { get; set; }
        public long? PROPERTIES_NEED_ID { get; set; }
        public string? TR_PROGRAM_CODE { get; set; }
        public long? TR_TYPE_ID { get; set; }
        public bool? IS_PLAN { get; set; }
        public long? TR_TRAIN_FIELD { get; set; }
        public bool? TR_COMMIT { get; set; }
        public bool? CERTIFICATE { get; set; }
        public long? CERTIFICATE_NAME { get; set; }
        public string? ATTACHED_FILE { get; set; }
        public bool? IS_PUBLIC { get; set; }
        public long? EXPECT_CLASS { get; set; }
        public long? TR_CURRENCY_ID { get; set; }
        public bool? TR_AFTER_TRAIN { get; set; }
        public DateTime? DAY_REVIEW_1 { get; set; }
        public DateTime? DAY_REVIEW_2 { get; set; }
        public DateTime? DAY_REVIEW_3 { get; set; }
        public long? PUBLIC_STATUS { get; set; }
        public DateTime? PORTAL_REGIST_FROM { get; set; }
        public DateTime? PORTAL_REGIST_TO { get; set; }
        public long? ASS_EMP1_ID { get; set; }
        public long? ASS_EMP2_ID { get; set; }
        public long? ASS_EMP3_ID { get; set; }
        public DateTime? ASS_DATE { get; set; }
        public string? CONTENT { get; set; }
        public string? TARGET_TRAIN { get; set; }
        public string? NOTE { get; set; }
    }
}
