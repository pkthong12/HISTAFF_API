using API.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("HU_WORK_LOCATION")]
public partial class HU_WORK_LOCATION: BASE_ENTITY
{
    public string? CODE { get; set; }

    public string? NAME { get; set; }

    public string? ADDRESS { get; set; }

    public long? PROVINCE { get; set; }

    public long? DISTRICT { get; set; }

    public long? WARD { get; set; }

    public string? PHONE { get; set; }

    public string? FAX { get; set; }

    public string? NOTE { get; set; }
}
