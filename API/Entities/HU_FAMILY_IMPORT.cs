using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Main;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace API.Entities;
[Table("HU_FAMILY_IMPORT")]

[PrimaryKey(nameof(XLSX_USER_ID), nameof(XLSX_EX_CODE), nameof(XLSX_SESSION), nameof(XLSX_ROW))]
public partial class HU_FAMILY_IMPORT : BASE_IMPORT
{
    public long? PROFILE_ID { get; set; }
    public long? EMPLOYEE_ID { get; set; }

    public long? RELATIONSHIP_ID { get; set; }

    public string? FULLNAME { get; set; }

    public long? GENDER { get; set; }

    public DateTime? BIRTH_DATE { get; set; }

    public string? PIT_CODE { get; set; }

    public bool? SAME_COMPANY { get; set; }

    public bool? IS_DEAD { get; set; }

    public bool? IS_DEDUCT { get; set; }

    public DateTime? DEDUCT_FROM { get; set; }

    public DateTime? DEDUCT_TO { get; set; }

    public DateTime? REGIST_DEDUCT_DATE { get; set; }

    public bool? IS_HOUSEHOLD { get; set; }

    public string? ID_NO { get; set; }

    public string? CAREER { get; set; }

    public long? NATIONALITY { get; set; }

    public string? BIRTH_CER_PLACE { get; set; }

    public long? BIRTH_CER_PROVINCE { get; set; }

    public long? BIRTH_CER_DISTRICT { get; set; }

    public long? BIRTH_CER_WARD { get; set; }

    public string? UPLOAD_FILE { get; set; }

    public long? STATUS_ID { get; set; }

    public string? NOTE { get; set; }
}