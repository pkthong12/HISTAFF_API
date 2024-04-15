using API.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_WORKING_IMPORT")]
[PrimaryKey(nameof(XLSX_USER_ID), nameof(XLSX_EX_CODE), nameof(XLSX_SESSION), nameof(XLSX_ROW))]
public partial class HU_WORKING_IMPORT : BASE_IMPORT
{
    public long? ID { get; set; }

    public long? TENANT_ID { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public long? POSITION_ID { get; set; }

    public long? ORG_ID { get; set; }

    public DateTime? EFFECT_DATE { get; set; }

    public DateTime? EXPIRE_DATE { get; set; }

    public string? DECISION_NO { get; set; }

    public long? TYPE_ID { get; set; }

    public long? SALARY_TYPE_ID { get; set; }

    public long? SALARY_SCALE_ID { get; set; }

    public long? SALARY_RANK_ID { get; set; }

    public long? SALARY_LEVEL_ID { get; set; }

    public decimal? COEFFICIENT { get; set; }

    public decimal? SAL_BASIC { get; set; }

    public decimal? SAL_TOTAL { get; set; }

    public decimal? SAL_PERCENT { get; set; }

    public bool? IS_CHANGE_SAL { get; set; }

    public long? SIGN_ID { get; set; }

    public string? SIGNER_NAME { get; set; }

    public string? SIGNER_POSITION { get; set; }

    public DateTime? SIGN_DATE { get; set; }

    public string? NOTE { get; set; }

    public long? STATUS_ID { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public long? ORGANIZATION_ID { get; set; }

    public int? IS_WAGE { get; set; }

    public DateTime? EXPIRE_UPSAL_DATE { get; set; }

    public int? IS_BHXH { get; set; }

    public int? IS_BHYT { get; set; }

    public int? IS_BHTNLD_BNN { get; set; }

    public int? IS_BHTN { get; set; }

    public long? SALARY_SCALE_DCV_ID { get; set; }

    public decimal? COEFFICIENT_DCV { get; set; }

    public decimal? SHORT_TEMP_SALARY { get; set; }

    public long? TAXTABLE_ID { get; set; }

    public long? SALARY_RANK_DCV_ID { get; set; }

    public long? SALARY_LEVEL_DCV_ID { get; set; }

    public string? ATTACHMENT { get; set; }

    public long? EMPLOYEE_OBJ_ID { get; set; }

    public long? LABOR_OBJ_ID { get; set; }

    public long? WAGE_ID { get; set; }

    public bool? IS_RESPONSIBLE { get; set; }

    public DateTime? BASE_DATE { get; set; }

    public DateTime? ISSUED_DATE { get; set; }

    public DateTime? CREATED_DATE_DECISION { get; set; }

    public decimal? SAL_INSU { get; set; }

    public DateTime? EFFECT_UPSAL_DATE { get; set; }

    public string? REASON_UPSAL { get; set; }
    public long? CUR_POSITION_ID { get; set; }

}
