using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("TR_PLAN")]
    public class TR_PLAN: BASE_ENTITY
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
        public string? FORM_TRAINING { get; set; }
        public string? ADDRESS_TRAINING { get; set; }
        public long? CENTER_ID { get; set; }
        public string? NOTE { get; set; }
        public string? FILENAME { get; set; }
        public string? CODE { get; set; }
    }
}
