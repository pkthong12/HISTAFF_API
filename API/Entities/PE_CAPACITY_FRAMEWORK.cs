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
[Table("PE_CAPACITY_FRAMEWORK")]

public partial class PE_CAPACITY_FRAMEWORK : BASE_ENTITY
{
    public int? RATIO_FROM { get; set; }
    public int? RATIO_TO { get; set; }
    public string? RATING {  get; set; }
    public long? SCORE_NOT_REQUIRED { get; set; }
    public string? DESCRIPTION { get; set; }
    public bool? IS_ACTIVE { get; set; }
}