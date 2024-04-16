using System;
using System.Collections.Generic;

namespace API.Entities;

public partial class SYS_MIGRATION_LOG
{
    public long ID { get; set; }

    public string TABLE_NAME { get; set; } = null!;

    public long? OLD_ID { get; set; }

    public long? NEW_ID { get; set; }

    public string? OLD_STRING_ID { get; set; }

    public string? NEW_STRING_ID { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? CREATED_BY { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public string? UPDATED_BY { get; set; }
}
