using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("INS_MATERNITY_MNG")]

public class INS_MATERNITY_MNG : BASE_ENTITY 
{
    public long? EMPLOYEE_ID { get; set; }
    public DateTime? NGAY_DU_SINH { get; set; }
    public bool? IS_NGHI_THAI_SAN { get; set; }
    public DateTime? FROM_DATE { get; set; }
    public DateTime? TO_DATE { get; set; }
    public DateTime? NGAY_SINH { get; set; }
    public int? SO_CON { get; set; }
    public decimal? TIEN_TAM_UNG { get; set; }
    public string? REMARK { get; set; }
    public bool? IS_ACTIVE { get; set; }
    public bool? IS_DELETED { get; set; }
    public DateTime? FROM_DATE_ENJOY { get; set; }
    public DateTime? TO_DATE_ENJOY { get; set; }
    public DateTime? NGAY_DI_LAM_SOM { get; set; }
    public string? NOTE { get; set; }
}
