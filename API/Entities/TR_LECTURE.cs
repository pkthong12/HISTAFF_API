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
[Table("TR_LECTURE")]

public partial class TR_LECTURE : BASE_ENTITY
{
    public long? TR_CENTER_ID { get; set; }
    public string? TEACHER_CODE { get; set; }
    public string? TEACHER_NAME { get; set; }
    public string? PHONE_NUMBER { get; set; }
    public string? EMAIL { get; set; }
    public string? ADDRESS_CONTACT { get; set; }
    public string? SUPPLIER_CODE { get; set; }
    public string? SUPPLIER_NAME { get; set; }
    public string? WEBSITE { get; set; }
    public string? TYPE_OF_SERVICE { get; set; }
    public string? NAME_OF_FILE { get; set; }
    public bool? IS_INTERNAL_TEACHER { get; set; }
    public string? NOTE { get; set; }
    public bool? IS_APPLY { get; set; }
}