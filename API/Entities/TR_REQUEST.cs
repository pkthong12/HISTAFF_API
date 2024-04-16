using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using Org.BouncyCastle.Tls;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("TR_REQUEST")]
    public class TR_REQUEST : BASE_ENTITY
    {
        public long? ORG_ID { get; set; }
        public DateTime? REQUEST_DATE { get; set; }
        public int? YEAR { get; set; }
        public long? TR_PLAN_ID { get; set; }
        public DateTime? EXPECTED_DATE { get; set; }
        public DateTime? START_DATE { get; set; }
        public decimal? EXPECTED_COST { get; set; }
        public long? TR_CURRENCY_ID { get; set; }
        public long? STATUS_ID { get; set; }
        public long? REQUEST_SENDER_ID { get; set; }
        public bool? IS_IRREGULARLY { get; set; }
        public long? TR_COURSE_ID { get; set; }
        public long? TRAIN_FORM_ID { get; set; }
        public long? PROPERTIES_NEED_ID { get; set; }
        public long? TR_UNIT_ID { get; set; }
        public string? VENUE { get; set; }
        public string? ATTACH_FILE { get; set; }
        public string? REJECT_REASON { get; set; }
        public DateTime? EXPECT_DATE_TO { get; set; }
        public long? SENDER_TITLE_ID { get; set; }
        public string? OTHER_COURSE { get; set; }
        public int? TRAINER_NUMBER { get; set; }
        public bool? TR_COMMIT { get; set; }
        public bool? CERTIFICATE { get; set; }
        public string? TR_PLACE { get; set; }
        public string? TEACHERS_ID { get; set; }
        public string? CENTERS_ID { get; set; }
        public bool? IS_PORTAL { get; set; }
        public bool? IS_APPROVE { get; set; }
        public string? REASON_PORTAL { get; set; }
        public string? REQUEST_CODE { get; set; }
        public string? CONTENT { get; set; }
        public string? REMARK { get; set; }
        public string? TARGET_TRAIN { get; set; }
    }
}
