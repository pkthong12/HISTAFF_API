using API.Main;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Entities
{
    [Table("TR_PLAN")]
    public class TR_PLAN : BASE_ENTITY
    {
        public string? NAME { get; set; }
        public string? YEAR { get; set; }
        public long? ORG_ID { get; set; }
        public DateTime? START_DATE_PLAN { get; set; }
        public DateTime? END_DATE_PLAN { get; set; }
        public DateTime? START_DATE_REAL { get; set; }
        public DateTime? END_DATE_REAL { get; set; }
        public int? PERSON_NUM_REAL { get; set; }
        public int? PERSON_NUM_PLAN { get; set; }

        public long? EXPECTED_COST { get; set; }
        public long? ACTUAL_COST { get; set; }
        public long? COURSE_ID { get; set; }
        public string? CONTENT { get; set; }
        public long? FORM_TRAINING_ID { get; set; }
        public long? PROPERTIES_NEED_ID { get; set; }
        public string? ADDRESS_TRAINING { get; set; }
        public long? CENTER_ID { get; set; }
        public string? NOTE { get; set; }
        public string? FILENAME { get; set; }
        public string? CODE { get; set; }
        public string? EXPECT_CLASS { get; set; }
        public bool? IS_COMMIT_TRAIN { get; set; }
        public bool? IS_CERTIFICATE { get; set; }
        public string? CERTIFICATE_NAME { get; set; }
        public long? TYPE_TRAINING_ID { get; set; }
        public bool? IS_POST_TRAIN { get; set; }
        public string? JOB_FAMILY_IDS { get; set; }
        public string? JOB_IDS { get; set; }
        public long? UNIT_MONEY_ID { get; set; }

        public DateTime? EVALUATION_DUE_DATE1 { get; set; }
        public DateTime? EVALUATION_DUE_DATE2 { get; set; }
        public DateTime? EVALUATION_DUE_DATE3 { get; set; }
    }
}
