using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("RPT_JOB_POS_HI")]
public partial class RPT_JOB_POS_HI
{
    public decimal id { get; set; }

    /// <summary>
    /// ID Node cha (OrgId)
    /// </summary>
    public decimal? parent_id { get; set; }

    /// <summary>
    /// Ten node (OrgName/JobName)
    /// </summary>
    public string? name_en { get; set; }

    /// <summary>
    /// Ten node (OrgName/JobName)
    /// </summary>
    public string? name_vn { get; set; }

    /// <summary>
    /// Ten nhom job
    /// </summary>
    public string? jobgroup { get; set; }

    /// <summary>
    /// Ma Org/Job
    /// </summary>
    public string? code { get; set; }

    /// <summary>
    /// Cap Org
    /// </summary>
    public decimal? level1 { get; set; }

    /// <summary>
    /// LY_FTE
    /// </summary>
    public decimal? ly_fte { get; set; }

    /// <summary>
    /// YTD_FTE
    /// </summary>
    public decimal? ytd_fte { get; set; }

    /// <summary>
    /// PLAN_FTE
    /// </summary>
    public decimal? plan_fte { get; set; }

    /// <summary>
    /// VS_LY_FTE
    /// </summary>
    public decimal? vs_ly_fte { get; set; }

    /// <summary>
    /// VS_PLAN_FTE
    /// </summary>
    public decimal? vs_plan_fte { get; set; }

    /// <summary>
    /// Org &gt;=1 co node con; Job &gt;= 0 k co node con
    /// </summary>
    public decimal? node_has_child { get; set; }

    /// <summary>
    /// Sap xep
    /// </summary>
    public decimal? order1 { get; set; }

    public string? created_by { get; set; }

    public DateTime? created_date { get; set; }

    /// <summary>
    /// Gia tri sau khi sua
    /// </summary>
    public decimal? ly_fte_v2 { get; set; }
}
