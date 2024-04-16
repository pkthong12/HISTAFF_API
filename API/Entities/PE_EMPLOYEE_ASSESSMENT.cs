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
[Table("PE_EMPLOYEE_ASSESSMENT")]

public partial class PE_EMPLOYEE_ASSESSMENT : BASE_ENTITY
{
    public long? EMPLOYEE_ID { get; set; }
    public long? HU_COMPETENCY_PERIOD_ID { get; set; }
}