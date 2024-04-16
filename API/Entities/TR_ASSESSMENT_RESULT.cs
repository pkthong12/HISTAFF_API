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
[Table("TR_ASSESSMENT_RESULT")]

public partial class TR_ASSESSMENT_RESULT : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }
    public long? STATUS_ID { get; set; }
    public bool? IS_LOCKED { get; set; }
    public string? QUESTION1 { get; set; }
    public string? QUESTION2 { get; set; }
    public string? QUESTION3 { get; set; }
    public string? QUESTION4 { get; set; }
    public long? TR_PROGRAM_ID { get; set; }
}