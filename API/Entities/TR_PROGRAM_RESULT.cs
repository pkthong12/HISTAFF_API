using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("TR_PROGRAM_RESULT")]
    public class TR_PROGRAM_RESULT : BASE_ENTITY
    {
        public long? TR_PROGRAM_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }
        public long? DURATION { get; set; }
        public bool? IS_EXAMS { get; set; }
        public bool? IS_END { get; set; }
        public bool? IS_REACH { get; set; }
        public bool? IS_CERTIFICATE { get; set; }
        public DateTime? CERTIFICATE_DATE { get; set; }
        public string? CERTIFICATE_NO { get; set; }
        public long? TR_RANK_ID { get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
        public long? TOIEC_BENCHMARK { get; set; }
        public long? TOIEC_SCORE_IN { get; set; }
        public long? TOIEC_SCORE_OUT { get; set; }
        public long? INCREMENT_SCORE { get; set; }
        public long? ABSENT_REASON { get; set; }
        public long? ABSENT_UNREASON { get; set; }
        public DateTime? CER_RECEIVE_DATE { get; set; }
        public DateTime? COMMIT_STARTDATE { get; set; }
        public DateTime? COMMIT_ENDDATE { get; set; }
        public long? COMMIT_WORKMONTH { get; set; }
        public bool? IS_REFUND_FEE { get; set; }
        public bool? IS_RESERVES { get; set; }
        public long? FINAL_SCORE { get; set; }
        public long? RETEST_SCORE { get; set; }
        public long? RETEST_RANK_ID { get; set; }
        public string? ATTACH_FILE { get; set; }
        public string? NOTE { get; set; }
        public string? RETEST_REMARK { get; set; }
        public long? RETEST_FEE { get; set; }
        public long? RESERVES_PERIOD { get; set; }
        public string? COMMENT_1 { get; set; }
        public string? COMMENT_2 { get; set; }
        public string? COMMENT_3 { get; set; }
        public bool? INSERT_HSNV { get; set; }
        public DateTime? CERT_DATE { get; set; }
    }
}
