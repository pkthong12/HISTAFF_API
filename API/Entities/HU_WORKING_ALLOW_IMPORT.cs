using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Main;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace API.Entities;
[Table("HU_WORKING_ALLOW_IMPORT")]

[PrimaryKey(nameof(XLSX_USER_ID), nameof(XLSX_EX_CODE), nameof(XLSX_SESSION), nameof(XLSX_ROW))]
public partial class HU_WORKING_ALLOW_IMPORT : BASE_IMPORT
{
    public long? ID { get; set; }

    public long? WORKING_ID { get; set; }
    public decimal? AMOUNT { get; set; }


    // 1
    public long? ALLOWANCE_ID1 { get; set; }
    public decimal? COEFFICIENT1 { get; set; }
    public DateTime? EFFECT_DATE1 { get; set; }
    public DateTime? EXPIRE_DATE1 { get; set; }


    // 2
    public long? ALLOWANCE_ID2 { get; set; }
    public decimal? COEFFICIENT2 { get; set; }
    public DateTime? EFFECT_DATE2 { get; set; }
    public DateTime? EXPIRE_DATE2 { get; set; }


    // 3
    public long? ALLOWANCE_ID3 { get; set; }
    public decimal? COEFFICIENT3 { get; set; }
    public DateTime? EFFECT_DATE3 { get; set; }
    public DateTime? EXPIRE_DATE3 { get; set; }


    // 4
    public long? ALLOWANCE_ID4 { get; set; }
    public decimal? COEFFICIENT4 { get; set; }
    public DateTime? EFFECT_DATE4 { get; set; }
    public DateTime? EXPIRE_DATE4 { get; set; }


    // 5
    public long? ALLOWANCE_ID5 { get; set; }
    public decimal? COEFFICIENT5 { get; set; }
    public DateTime? EFFECT_DATE5 { get; set; }
    public DateTime? EXPIRE_DATE5 { get; set; }
}