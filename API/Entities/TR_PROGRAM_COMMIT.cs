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
[Table("TR_PROGRAM_COMMIT")]

public partial class TR_PROGRAM_COMMIT : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }
    public decimal? CALCULATE_REIMBURSEMENT { get; set; }
    public string? COMMIT_NO { get; set; }
    public DateTime? SIGN_DATE { get; set; }
    public long? TRAINING_COSTS { get; set; }
    public long? MONEY_COMMIT { get; set; }
    public int? TIME_COMMIT { get; set; }
    public DateTime? DATE_FROM { get; set; }
    public DateTime? DATE_TO { get; set; }
    public int? DAY_QUANTITY { get; set; }
}