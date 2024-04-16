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
[Table("TR_REQUEST_YEAR")]

public partial class TR_REQUEST_YEAR : BASE_ENTITY
{
    public int? YEAR { get; set; }
    public long? QUARTER_ID { get; set; }
    public long? ORG_ID { get; set; }
    public DateTime? DATE_OF_REQUEST { get; set; }
    public long? TR_COURSE_ID { get; set; }
    public string? CONTENT { get; set; }
    public long? COMPANY_ID { get; set; }
    public string? PARTICIPANTS { get; set; }
    public int? QUANTITY_PEOPLE { get; set; }
    public long? INITIALIZATION_LOCATION { get; set; }
    public long? PRIORITY_LEVEL { get; set; }
    public long? STATUS_ID { get; set; }
    public decimal? MONEY { get; set; }
    public string? NOTE { get; set; }
}