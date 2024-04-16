namespace API.Entities.REPORT
{
    public class STATISTICS_OF_WORKERS_BY_GENDER_AGE_REPORT
    {
        public long STT { get; set; }
        public string? ORG_NAME { get; set; }
    }

    public class EMPLOYEE_GENDER_AGE
    {
        public long? ID { get; set; }
        public string? CODE { get; set; }
        public string? POSITION_NAME { get; set; }
        public long? ORG_ID { get; set; }
        public string? ORG_NAME { get; set; }
        public decimal? ORDER_NUM { get; set; }
        public string? GENDER_CODE { get; set; }
        public DateTime? BIRTH_DATE { get; set; }
    }

    public class MODEL_RESULT
    {
        public long? STT { get; set; }
        public string? ORG_NAME { get; set; }
        public long? TOTAL_NUMBER { get; set; }
        
        public long? COUNT_MALE { get; set; }
        public decimal? PERCENT_MALE { get; set; }
        
        public long? COUNT_FEMALE { get; set; }
        public decimal? PERCENT_FEMALE { get; set; }
        
        public long? COUNT_AGE_18_TO_30 { get; set; }
        public decimal? PERCENT_AGE_18_TO_30 { get; set; }

        public long? COUNT_AGE_31_TO_35 { get; set; }
        public decimal? PERCENT_AGE_31_TO_35 { get; set; }

        public long? COUNT_AGE_36_TO_40 { get; set; }
        public decimal? PERCENT_AGE_36_TO_40 { get; set; }

        public long? COUNT_AGE_41_TO_50 { get; set; }
        public decimal? PERCENT_AGE_41_TO_50 { get; set; }

        public long? COUNT_AGE_51_TO_60 { get; set; }
        public decimal? PERCENT_AGE_51_TO_60 { get; set; }

        public long? COUNT_AGE_GREATER_THAN_60 { get; set; }
        public decimal? PERCENT_AGE_GREATER_THAN_60 { get; set; }
    }
}