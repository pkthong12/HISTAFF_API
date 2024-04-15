using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("AT_SWIPE_DATA_IMPORT")]
[PrimaryKey(nameof(XLSX_USER_ID), nameof(XLSX_EX_CODE), nameof(XLSX_SESSION), nameof(XLSX_ROW))]
public partial class AT_SWIPE_DATA_IMPORT
{
    public string? XLSX_USER_ID { get; set; }

    public string? XLSX_EX_CODE { get; set; }

    public DateTime? XLSX_INSERT_ON { get; set; }

    public long? XLSX_SESSION { get; set; }

    public string? XLSX_FILE_NAME { get; set; }

    public int? XLSX_ROW { get; set; }

    public long? ID { get; set; }

    /// <summary>
    /// Mã chấm công
    /// </summary>
    public string? ITIME_ID { get; set; }

    public long? TERMINAL_ID { get; set; }

    public DateTime? WORKING_DAY { get; set; }

    public DateTime? VALTIME { get; set; }
    public string? TIME_ONLY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? CREATED_BY { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public string? UPDATED_BY { get; set; }

    public string? CREATED_LOG { get; set; }

    public string? UPDATED_LOG { get; set; }

    public long? ORG_ID { get; set; }

    public int? EVT { get; set; }

    public int? IS_GPS { get; set; }

    public int? ADDRESS_ID { get; set; }

    public long? EMP_ID { get; set; }

    public string? LATATUIDE_LONGTATUIDE { get; set; }

    public int? SHIFT_ID { get; set; }
}
