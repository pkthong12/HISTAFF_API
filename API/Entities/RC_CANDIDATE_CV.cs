using API.Main;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Ocsp;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Entities;
[Table("RC_CANDIDATE_CV")]
public class RC_CANDIDATE_CV : BASE_ENTITY
{
    public long? CANDIDATE_ID { get; set; }
    public long? GENDER_ID{get; set;}
    public long? MARITAL_STATUS{get; set;}
    public long? NATION_ID{get; set;}
    public long? RELIGION_ID{get; set;}
    public DateTime? BIRTH_DATE{get; set;}
    public string? BIRTH_ADDRESS{get; set;}
    public long? NATIONALITY_ID{get; set;}
    public string? ID_NO{get; set;}
    public DateTime? ID_DATE{get; set;}
    public DateTime? ID_DATE_EXPIRE{get; set;}
    public long? ID_PLACE{get; set;}
    public string? PER_ADDRESS{get; set;}
    public long? PER_PROVINCE{get; set;}
    public long? PER_DISTRICT{get; set;}
    public long? PER_WARD{get; set;}
    public string? CONTACT_ADDRESS_TEMP{get; set;}
    public long? CONTACT_PROVINCE_TEMP{get; set;}
    public long? CONTACT_DISTRICT_TEMP{get; set;}
    public long? CONTACT_WARD_TEMP{get; set;}
    public string? PER_EMAIL{get; set;}
    public string? MOBILE_PHONE{get; set;}
    public string? FINDER_SDT{get; set;}
    public bool? IS_WORK_PERMIT{get; set;}
    public DateTime? WORK_PERMIT_START{get; set;}
    public DateTime? WORK_PERMIT_END{get; set;}
    public long? EDUCATION_LEVEL_ID{get; set;}
    public long? LEARNING_LEVEL_ID{get; set;}
    public long? GRADUATE_SCHOOL_ID{get; set;}
    public long? MAJOR_ID{get; set;}
    public int? YEAR_GRADUATION{get; set;}
    public string? RATING{get; set;}
    public long? RC_COMPUTER_LEVEL_ID{get; set;}
    public long? TYPE_CLASSIFICATION_ID{get; set;}
    public long? LANGUAGE_ID{get; set;}
    public long? LANGUAGE_LEVEL_ID{get; set;}
    public string? MARK{get; set;}
    public long? POS_WISH1_ID{get; set;}
    public long? POS_WISH2_ID{get; set;}
    public decimal? PROBATION_SALARY{get; set;}
    public decimal? WISH_SALARY{get; set;}
    public string? DESIRED_WORKPLACE{get; set;}
    public DateTime? START_DATE_WORK{get; set;}
    public string? LEVEL_DESIRED{get; set;}
    public string? NUM_EXPERIENCE{get; set;}
    public bool? IS_HSV_HV{get; set;}
    public string? OTHER_SUGGESTIONS{get; set;}
    public string? IMAGE{get; set;}
    public DateTime? CREATED_LOG{get; set;}
    public string? UPDATED_LOG{get; set;}
    public string? RE_NAME { get; set; }
    public long? RE_RELATIONSHIP { get; set;}
    public string? RE_PHONE { get; set; }
    public string? RE_ADDRESS { get; set; }
    public string? IN_NAME { get; set; }
    public string? IN_PHONE { get; set; }
    public string? IN_NOTE { get; set; }
    public double? HEIGHT { get; set; }
    public string? EAR_NOSE_THROAT { get; set; }
    public double? WEIGHT { get; set; }
    public string? DENTOMAXILLOFACIAL { get; set; }
    public string? BLOOD_GROUP { get; set; }
    public string? HEART { get; set; }
    public string? BLOOD_PRESSURE { get; set; }
    public string? LUNGS_AND_CHEST { get; set; }
    public string? LEFT_EYE_VISION { get; set; }
    public string? RIGHT_EYE_VISION { get; set; }
    public string? HEPATITIS_B { get; set; }
    public string? LEATHER_VENEREAL { get; set; }
    public long? HEALTH_TYPE { get; set; }
    public string? NOTE_SK { get; set; }
}
