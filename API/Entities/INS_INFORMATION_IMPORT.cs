using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Main;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace API.Entities;
[Table("INS_INFORMATION_IMPORT")]

[PrimaryKey(nameof(XLSX_USER_ID), nameof(XLSX_EX_CODE), nameof(XLSX_SESSION), nameof(XLSX_ROW))]
public partial class INS_INFORMATION_IMPORT : BASE_IMPORT
{
    public long? EMPLOYEE_ID { get; set; }
    public long? PROFILE_ID { get; set; }
    public int? SENIORITY_INSURANCE { get; set; }

    //BHXH
    public DateTime? BHXH_FROM_DATE { get; set; }
    public DateTime? BHXH_TO_DATE { get; set; }
    public string? BHXH_NO { get; set; }
    public long? BHXH_STATUS_ID { get; set; }
    public DateTime? BHXH_SUPPLIED_DATE { get; set; }
    public DateTime? BHXH_REIMBURSEMENT_DATE { get; set; }
    public DateTime? BHXH_GRANT_DATE { get; set; }
    public string? BHXH_DELIVERER { get; set; }
    public string? BHXH_STORAGE_NUMBER { get; set; }
    public string? BHXH_RECEIVER { get; set; }
    public string? BHXH_NOTE { get; set; }

    //BHYT
    public string? BHYT_NO { get; set; }
    public DateTime? BHYT_FROM_DATE { get; set; }
    public DateTime? BHYT_TO_DATE { get; set; }
    public DateTime? BHYT_EFFECT_DATE { get; set; }
    public DateTime? BHYT_EXPIRE_DATE { get; set; }
    public long? BHYT_STATUS_ID { get; set; }
    public long? BHYT_WHEREHEALTH_ID { get; set; }
    public DateTime? BHYT_RECEIVED_DATE { get; set; }
    public string? BHYT_RECEIVER { get; set; }
    public DateTime? BHYT_REIMBURSEMENT_DATE { get; set; }

    //BHTN
    public DateTime? BHTN_FROM_DATE { get; set; }
    public DateTime? BHTN_TO_DATE { get; set; }

    //BHTNLD-BNN
    public DateTime? BHTNLD_BNN_FROM_DATE { get; set; }
    public DateTime? BHTNLD_BNN_TO_DATE { get; set; }
}