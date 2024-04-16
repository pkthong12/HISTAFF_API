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
[Table("HU_BLACKLIST")]

public partial class HU_BLACKLIST : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }
    public DateTime? DATE_JOIN { get; set; }
    public DateTime? DATE_SEND_FORM { get; set; }
    public DateTime? END_DATE_WORK { get; set; }
    public DateTime? LAST_WORKING_DAY { get; set; }
    public long? STATUS_ID { get; set; }
    public string? NOTE { get; set; }
    public string? REASON_FOR_BLACKLIST { get; set; }
}