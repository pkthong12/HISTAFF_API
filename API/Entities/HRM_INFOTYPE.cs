using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("HRM_INFOTYPE")]
public partial class HRM_INFOTYPE
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME_CODE { get; set; } = null!;

    public string? NAME_EN { get; set; }

    public string? NAME_VN { get; set; }

    public string? DESCRIPTION { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? CREATED_BY { get; set; }

    public DateTime? UPDATE_DATE { get; set; }

    public string? UPDATE_BY { get; set; }
}
