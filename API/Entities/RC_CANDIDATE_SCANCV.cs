using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("RC_CANDIDATE_SCANCV")]
public partial class RC_CANDIDATE_SCANCV
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }

    public string? FULLNAME_VN { get; set; }

    public DateTime? BIRTH_DATE { get; set; }

    public DateTime? CREATED_DATE { get; set; }

    public string? CREATED_BY { get; set; }

    public string? CREATED_LOG { get; set; }

    public DateTime? UPDATED_DATE { get; set; }

    public string? UPDATED_BY { get; set; }

    public string? UPDATED_LOG { get; set; }

    public string? GENDER { get; set; }

    public string? ADDRESS { get; set; }

    public string? SKILL { get; set; }

    public string? DESCRIPTION { get; set; }

    public string? EMAIL { get; set; }

    public string? MOBILEPHONE { get; set; }
}
