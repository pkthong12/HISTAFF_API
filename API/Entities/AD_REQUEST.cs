using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("AD_REQUESTS")]
[Keyless]
public partial class AD_REQUEST
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long REQUEST_ID { get; set; }

    public long? PROGRAM_ID { get; set; }

    public string? PHASE_CODE { get; set; }

    public string? STATUS_CODE { get; set; }

    public DateTime? START_DATE { get; set; }

    public DateTime? END_DATE { get; set; }

    public DateTime? ACTUAL_START_DATE { get; set; }

    public DateTime? ACTUAL_COMPLETE_DATE { get; set; }

    public decimal? OUTPUTFILE_SIZE { get; set; }

    public string? NLS_LANGUAGE { get; set; }

    public string? NLS_TERRITORY { get; set; }

    public string? PRINTER { get; set; }

    public decimal? size { get; set; }

    public decimal? ORIENTATION { get; set; }

    public decimal? PERMISSION { get; set; }

    public string? CREATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? MODIFIED_BY { get; set; }

    public DateTime? MODIFIED_DATE { get; set; }

    public string? CREATED_LOG { get; set; }

    public string? MODIFIED_LOG { get; set; }

    public string? STORE_EXECUTE_IN { get; set; }

    public string? STORE_EXECUTE_OUT { get; set; }

    public decimal? PRIORITY { get; set; }

    public string? LOG_FILE { get; set; }

    public string? TEMPLATE_NAME { get; set; }

    public string? TEMPLATE_TYPE_IN { get; set; }

    public string? TEMPLATE_TYPE_OUT { get; set; }

    public string? TEMPLATE_URL { get; set; }

    public string? FILE_OUT_URL { get; set; }

    public string? FILE_OUT_NAME { get; set; }

    public string? CSV_FILE { get; set; }

    public decimal? TRY_COUNT { get; set; }
}
