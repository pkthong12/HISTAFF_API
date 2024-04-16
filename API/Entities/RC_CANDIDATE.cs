using API.Main;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("RC_CANDIDATE")]
public class RC_CANDIDATE : BASE_ENTITY
{
    public string? CANDIDATE_CODE { get; set; }
    public long? GENDER_ID { get; set; }
    public string? FULLNAME_VN { get; set; }
    public long? ORG_ID { get; set; }
    public long? TITLE_ID { get; set; }
    public DateTime? JOIN_DATE { get; set; }
    public string? CREATED_LOG { get; set; }
    public string? UPDATED_LOG { get; set; }
    public DateTime? EFFECT_DATE { get; set; }
    public string? TITLE_NAME { get; set; }
    public string? FILE_NAME { get; set; }
    public long? RC_PROGRAM_ID { get; set; }
    public bool? IS_PONTENTIAL { get; set; }
    public bool? IS_BLACKLIST { get; set; }
    public bool? IS_REHIRE { get; set; }
    public string? STATUS_ID { get; set; }
    public string? EMPLOYEE_CODE { get; set; }
    public string? CARE_TITLE_NAME { get; set; }
    public string? RECRUIMENT_WEBSITE { get; set; }
    public long? RC_SOURCE_REC_ID { get; set; }
    public long? WANTED_LOCATION1 { get; set; }
    public long? WANTED_LOCATION2 { get; set; }
    public decimal? LEVEL_SALARY_WISH { get; set; }
    public bool? IS_WORK_PERMIT { get; set; }
    public DateTime? PERMIT_END_DATE { get; set; }
    public DateTime? PERMIT_START_DATE { get; set; } 
    public string? WORK_PERMIT_NO { get; set; }
    public long? PROFILE_ID { get; set; }

    [ForeignKey("PROFILE_ID")]
    public virtual RC_CANDIDATE_CV? Profile { get; set; }
}
