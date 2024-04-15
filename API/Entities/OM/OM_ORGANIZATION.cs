using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities.OM;
[Table("OM_ORGANIZATION")]
public partial class OM_ORGANIZATION: BASE_ENTITY
{
    public long? TENANT_ID { get; set; }

    public string CODE { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public long? PARENT_ID { get; set; }
    
    public long? COMPANY_ID { get; set; }

    public long? ORG_LEVEL_ID { get; set; }
    
    public int? ORDER_NUM { get; set; }

    public long? MNG_ID { get; set; }

    public DateTime? FOUNDATION_DATE { get; set; }

    public DateTime? DISSOLVE_DATE { get; set; }

    public string? PHONE { get; set; }

    public string? FAX { get; set; }

    public string? ADDRESS { get; set; }

    public string? BUSINESS_NUMBER { get; set; }

    public DateTime? BUSINESS_DATE { get; set; }

    public string? TAX_CODE { get; set; }

    public string? NOTE { get; set; }

    /// <summary>
    /// &apos;A&apos; : AP DUNG, &apos;I&apos;: NGUNG AP DUNG
    /// </summary>
    public string? STATUS { get; set; }

    /// <summary>
    /// TEN VIET TAT
    /// </summary>
    public string? SHORT_NAME { get; set; }

    public string? NAME_EN { get; set; }

    public double? UY_BAN { get; set; }
    
    public int? LEVEL_ORG { get; set; }

    public double? GROUPPROJECT { get; set; }
}
