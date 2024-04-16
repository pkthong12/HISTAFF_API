namespace API.Entities.REPORT
{
    public class PERSONAL_TRAINING_RESULTS_REPORT
    {
        public long STT { get; set; }
        public string? COURSE_CODE { get; set; }
        public string? COURSE_NAME { get; set; }
        public string? YEAR { get; set; }
        public string? TRAIN_FIELD { get; set; }
        public string? FORM_TRAINING { get; set; }
        public string? START_DATE_PLAN { get; set; }
        public string? END_DATE_PLAN { get; set; }
        public string? ADDRESS_TRAINING { get; set; }
        public long? EXPECTED_COST { get; set; }
        public string? NAME_CENTER { get; set; }
        public string? TYPE_TRANING { get; set; }
        public string? IS_CERTIFICATE { get; set; }
        public string? CERTIFICATE_NAME { get; set; }
    }

    public class PERSONAL_TRAINING_RESULTS_REPORT_EMP
    {
        public string? CODE { get; set; }
        public string? FULL_NAME { get; set; }
        public string? ORG_NAME { get; set; }
        public string? POS_NAME { get; set; }
        public string? BIRTH_DATE { get; set; }
        public string? GENDER { get; set; }
    }
}
