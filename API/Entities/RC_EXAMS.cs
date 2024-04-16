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
[Table("RC_EXAMS")]

public partial class RC_EXAMS : BASE_ENTITY
{
    public long? ORG_ID { get; set; }
    public long? POSITION_ID { get; set; }
    public string? NAME { get; set; }
    public int? POINT_LADDER { get; set; }
    public int? COEFFICIENT { get; set; }
    public int? POINT_PASS { get; set; }
    public int? EXAMS_ORDER { get; set; }
    public bool? IS_PV {  get; set; }
    public string? NOTE {  get; set; }
}