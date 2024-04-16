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
[Table("TR_TRANNING_RECORD")]

public partial class TR_TRANNING_RECORD : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }
    public long? TR_COURSE_ID { get; set; }
    public DateTime? DATE_FROM { get; set; }
    public DateTime? DATE_TO { get; set; }
    public string? CONTENT { get; set; }
    public string? TARGET_TEXT { get; set; }
    public string? TRAINING_PLACE { get; set; }
    public string? TRAINING_CENTER { get; set; }
    public string? RESULT { get; set; }
    public decimal? SCORES { get; set; }
    public string? RATING { get; set; }
    public string? EVALUATE_DUE1 { get; set; }
    public string? EVALUATE_DUE2 { get; set; }
    public string? EVALUATE_DUE3 { get; set; }
    public string? CERTIFICATE_TEXT { get; set; }
    public DateTime? CERTIFICATE_ISSUANCE_DATE { get; set; }
    public string? COMMITMENT_NUMBER { get; set; }
    public decimal? COMMITMENT_AMOUNT { get; set; }
    public int? MONTH_COMMITMENT { get; set; }
    public DateTime? COMMITMENT_START_DATE { get; set; }
    public DateTime? COMMITMENT_END_DATE { get; set; }
}