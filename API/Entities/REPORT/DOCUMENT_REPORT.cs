namespace API.Entities.REPORT
{
    public class DOCUMENT_REPORT
    {
        //public long STT { get; set; }
        public string? CODE { get; set; }
        public string? FULL_NAME { get; set; }
        public string? POSITION { get; set; }
        public string? ORGANIZATION { get; set; }
        public string? STATUS { get; set; }
        public int? TOLTAL_SUBMIT { get; set; }
        public string? BIRTH_CERTIFICATE { get; set; }//giấy khai sinh
        public string? DEGREE { get; set; }//bằng cấp
        public string? ID_NO { get; set; }//CCCD-CMND
        public string? CERTIFICATE_RECIDENCE_INF { get; set; }//Giấy xác nhận thông tin nơi cư trú
        public string? CURRICULUM_VITAE { get; set; }//Sơ yếu lý lịch
        public string? CV { get; set; }//Đơn xin việc
        public string? CERTIFICATE { get; set; }//Chứng chỉ
        public string? HEALTH_CERTIFICATION { get; set; }//Giấy chứng nhận sức khỏe
    }
}
