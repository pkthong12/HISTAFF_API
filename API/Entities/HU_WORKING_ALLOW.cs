using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_WORKING_ALLOW")]
public partial class HU_WORKING_ALLOW
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? ID { get; set; }

    public long? WORKING_ID { get; set; }

    public long ALLOWANCE_ID { get; set; }

    public decimal? AMOUNT { get; set; }

    public DateTime? EFFECT_DATE { get; set; }

    public DateTime? EXPIRE_DATE { get; set; }

    public decimal? COEFFICIENT { get; set; }



    // 1
    public long? ALLOWANCE_ID1 { get; set; }
    public decimal? COEFFICIENT1 { get; set; }
    public DateTime? EFFECT_DATE1 { get; set; }
    public DateTime? EXPIRE_DATE1 { get; set; }


    // 2
    public long? ALLOWANCE_ID2 { get; set; }
    public decimal? COEFFICIENT2 { get; set; }
    public DateTime? EFFECT_DATE2 { get; set; }
    public DateTime? EXPIRE_DATE2 { get; set; }


    // 3
    public long? ALLOWANCE_ID3 { get; set; }
    public decimal? COEFFICIENT3 { get; set; }
    public DateTime? EFFECT_DATE3 { get; set; }
    public DateTime? EXPIRE_DATE3 { get; set; }


    // 4
    public long? ALLOWANCE_ID4 { get; set; }
    public decimal? COEFFICIENT4 { get; set; }
    public DateTime? EFFECT_DATE4 { get; set; }
    public DateTime? EXPIRE_DATE4 { get; set; }


    // 5
    public long? ALLOWANCE_ID5 { get; set; }
    public decimal? COEFFICIENT5 { get; set; }
    public DateTime? EFFECT_DATE5 { get; set; }
    public DateTime? EXPIRE_DATE5 { get; set; }
}
