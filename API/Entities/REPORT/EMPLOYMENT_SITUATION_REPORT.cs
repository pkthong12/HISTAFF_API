namespace API.Entities.REPORT
{
    public class EMPLOYMENT_SITUATION_REPORT
    {
        public long STT { get; set; }
        public string? ORG_NAME { get; set; }
        public int AMOUNT_START_PERIOD { get; set; }
        public int AMOUNT_INCREASE { get; set; }
        public int AMOUNT_DECREASE { get; set; }
        public int AMOUNT_END_PERIOD { get; set; }
    }
}
