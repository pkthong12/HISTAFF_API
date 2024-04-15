using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HUV_ORGANIZATION")]
public partial class HUV_ORGANIZATION
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? ID { get; set; }

    public string? NAME { get; set; }

    public long? PARENT_ID { get; set; }

    public string? CODE { get; set; }

    public string? ADDRESS { get; set; }

    public string? SHORT_NAME { get; set; }

    public string? ORG_PATH { get; set; }

    public DateTime? DISSOLVE_DATE { get; set; }

    public int? HU_ORG_TITLEID { get; set; }

    public double uy_ban { get; set; }

    public int? ORG_ID1 { get; set; }

    public int? ORG_ID2 { get; set; }

    public int? ORG_ID3 { get; set; }

    public int? ORG_ID4 { get; set; }

    public int? ORG_ID5 { get; set; }

    public int? ORG_ID6 { get; set; }

    public int? ORG_ID7 { get; set; }

    public int? ORG_ID8 { get; set; }

    public int? ORG_ID9 { get; set; }

    public int? ORG_ID10 { get; set; }

    public int? ORG_ID11 { get; set; }

    public string? ORG_NAME1 { get; set; }

    public string? ORG_NAME2 { get; set; }

    public string? ORG_NAME3 { get; set; }

    public string? ORG_NAME4 { get; set; }

    public string? ORG_NAME5 { get; set; }

    public string? ORG_NAME6 { get; set; }

    public string? ORG_NAME7 { get; set; }

    public string? ORG_NAME8 { get; set; }

    public string? ORG_NAME9 { get; set; }

    public string? ORG_NAME10 { get; set; }

    public string? ORG_NAME11 { get; set; }

    public string? ORG_CODE1 { get; set; }

    public string? ORG_CODE2 { get; set; }

    public string? ORG_CODE3 { get; set; }

    public string? ORG_CODE4 { get; set; }

    public string? ORG_CODE5 { get; set; }

    public string? ORG_CODE6 { get; set; }

    public string? ORG_CODE7 { get; set; }

    public string? ORG_CODE8 { get; set; }

    public string? ORG_CODE9 { get; set; }

    public string? ORG_CODE10 { get; set; }

    public string? ORG_CODE11 { get; set; }

    public string? ACTFLG { get; set; }

    public int? LEVEL { get; set; }

    public decimal? MNG_ID { get; set; }

    public string? NAME_EN { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? STATUS { get; set; }

    public double? LEVEL_ORG { get; set; }
}
