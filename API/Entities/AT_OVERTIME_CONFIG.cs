using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("AT_OVERTIME_CONFIG")]
public partial class AT_OVERTIME_CONFIG
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public int? HOUR_MIN { get; set; }

    public int? HOUR_MAX { get; set; }

    public decimal? FACTOR_NT { get; set; }

    public decimal? FACTOR_NN { get; set; }

    public decimal? FACTOR_NL { get; set; }

    public decimal? FACTOR_DNT { get; set; }

    public decimal? FACTOR_DNN { get; set; }

    public decimal? FACTOR_DNL { get; set; }

    public string? CREATED_BY { get; set; }

    public string? UPDATED_BY { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public DateTime? UPDATED_DATE { get; set; }
}
