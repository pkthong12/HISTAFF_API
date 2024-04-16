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
[Table("AS_PROJECT")]

public partial class AS_PROJECT : BASE_ENTITY
{
    public string? CODE { get; set; }
    public string? NAME { get; set; }
    public string? DESCRIPTION { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_ACTIVE { get; set; }
}