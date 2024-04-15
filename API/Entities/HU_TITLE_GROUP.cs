using API.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("HU_TITLE_GROUP")]
public partial class HU_TITLE_GROUP: BASE_ENTITY
{

    public string? CODE { get; set; }

    public string? NAME { get; set; }

    public string? NOTE { get; set; }
}
