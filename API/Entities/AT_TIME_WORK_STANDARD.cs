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
[Table("AT_TIME_WORK_STANDARD")]

public partial class AT_TIME_WORK_STANDARD : BASE_ENTITY
{
    public int? EFFECTIVE_YEAR { get; set; }
    public long? ORG_ID { get; set; }
    public long? OBJ_EMPLOYEE_ID { get; set; }
    public long? WORK_ENVIRONMENT_ID { get; set; }
    public bool? IS_NOT_SATURDAY_INCLUDED { get; set; }
    public bool? IS_NOT_SUNDAY_INCLUDED { get; set; }
    public bool? IS_NOT_HALF_SATURDAY_INCLUDED { get; set; }
    public bool? IS_NOT_TWO_SATURDAYS { get; set; }
    public int? DEDUCT_WORK_DURING_MONTH { get; set; }
    public int? DEFAULT_WORK { get; set; }
    public int? COEFFICIENT { get; set; }
    public int? T1 { get; set; }
    public int? T2 { get; set; }
    public int? T3 { get; set; }
    public int? T4 { get; set; }
    public int? T5 { get; set; }
    public int? T6 { get; set; }
    public int? T7 { get; set; }
    public int? T8 { get; set; }
    public int? T9 { get; set; }
    public int? T10 { get; set; }
    public int? T11 { get; set; }
    public int? T12 { get; set; }
}