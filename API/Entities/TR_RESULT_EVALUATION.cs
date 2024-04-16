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
[Table("TR_RESULT_EVALUATION")]

public partial class TR_RESULT_EVALUATION : BASE_ENTITY
{
    public long? TR_SETTING_CRI_DETAIL_ID { get; set; }
    public int? POINT_EVALUATE { get; set; }
    public string? GENERAL_OPINION { get; set; }
    public long? TR_ASSESSMENT_RESULT_ID { get; set; }
}