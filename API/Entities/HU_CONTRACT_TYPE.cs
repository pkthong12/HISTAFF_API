using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_CONTRACT_TYPE")]
public partial class HU_CONTRACT_TYPE : BASE_ENTITY
{
    public string? CODE { get; set; }

    public string? NAME { get; set; }

    public int? PERIOD { get; set; }

    public int? DAY_NOTICE { get; set; }

    public string? NOTE { get; set; }

    public bool? IS_LEAVE { get; set; }

    public long? TYPE_ID { get; set; }

    public bool? IS_BHXH { get; set; }

    public bool? IS_BHYT { get; set; }

    public bool? IS_BHTN { get; set; }

    public bool? IS_BHTNLD_BNN { get; set; }

    public bool? IS_ACTIVE { get; set; }

    [ForeignKey("TYPE_ID")]
    public virtual SYS_CONTRACT_TYPE? ContractType { get; set; }
}
