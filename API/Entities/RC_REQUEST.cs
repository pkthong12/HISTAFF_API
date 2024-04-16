using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using API.Main;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Entities;
[Table("RC_REQUEST")]

public partial class RC_REQUEST : BASE_ENTITY
{
    // This is "General information" for "View Edit"
    public string? CODE { get; set; }
    public string? RESTER_NAME { get; set; }
    public bool? IS_IN_BOUNDARY { get; set; }
    public bool? IS_OUT_BOUNDARY { get; set; }
    public long? ORG_ID { get; set; }
    public long? POSITION_ID { get; set; }
    public string? POSITION_GROUP_OF_RECRUITMENT { get; set; }
    public long? PETITIONER_ID { get; set; }
    public DateTime? DATE_VOTE { get; set; }
    public DateTime? DATE_NEED_RESPONSE { get; set; }
    public long? RECRUITMENT_FORM { get; set; }
    public long? WORKPLACE { get; set; }
    public long? SALARY_LEVEL { get; set; }
    public int? QUANTITY_AVAILABLE { get; set; }
    public int? BOUNDARY_QUANTITY { get; set; }
    public int? PLAN_LEAVE { get; set; }
    public long? RECRUITMENT_REASON { get; set; }
    public int? NUMBER_NEED { get; set; }
    public bool? IS_BEYOND_BOUNDARY { get; set; }
    public bool? IS_REQUIRE_COMPUTER { get; set; }
    public string? DETAIL_EXPLANATION { get; set; }



    // This is "Details of recruitment requirements" for "View Edit"
    public long? EDUCATION_LEVEL_ID { get; set; }
    public long? SPECIALIZE_LEVEL_ID { get; set; }
    public string? OTHER_PROFESSIONAL_QUALIFICATIONS { get; set; }
    public int? AGE_FROM { get; set; }
    public int? AGE_TO { get; set; }
    public long? LANGUAGE_ID { get; set; }
    public long? LANGUAGE_LEVEL_ID { get; set; }
    public int? LANGUAGE_POINT { get; set; }
    public string? FOREIGN_LANGUAGE_ABILITY { get; set; }
    public int? MINIMUM_YEAR_EXP { get; set; }
    public long? GENDER_PRIORITY_ID { get; set; }
    public string? COMPUTER_LEVEL { get; set; }
    public string? JOB_DESCRIPTION { get; set; }
    public string? LEVEL_PRIORITY { get; set; }
    public string? NAME_OF_FILE { get; set; }
    public string? OTHER_REQUIRE { get; set; }
    public bool? IS_APPROVE { get; set; }
    public long? PERSON_IN_CHARGE { get; set; }
    public long? STATUS_ID { get; set; }
}