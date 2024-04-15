using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Main;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace API.Entities;
[Table("HU_WORKING_BEFORE")]


public partial class HU_WORKING_BEFORE : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }

    public string? COMPANY_NAME { get; set; }

    public string? TITLE_NAME { get; set; }

    public DateTime? FROM_DATE { get; set; }

    public DateTime? END_DATE { get; set; }

    public string? MAIN_DUTY { get; set; }

    public string? TER_REASON { get; set; }

    public string? SENIORITY { get; set; }

    public int? IS_NHANUOC {  get; set; }

    public long? ORG_ID { get; set; }

    public long? POSITION_ID { get; set; }
    public long? PROFILE_ID { get; set; }
}