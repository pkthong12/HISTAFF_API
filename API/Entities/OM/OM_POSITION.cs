using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities.OM;
[Table("OM_POSITION")]
public partial class OM_POSITION
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long ID { get; set; }
	public long? GROUP_ID { get; set; }
	public string? CODE { get; set; }
	public string? NAME { get; set; }
    public string? NAME_EN { get; set; }
	public string? NOTE { get; set; }
	public string? JOB_DESC { get; set; }
	public bool? IS_ACTIVE { get; set; }
	public string? CREATED_BY { get; set; }
	public string? UPDATED_BY { get; set; }
	public DateTime? CREATED_DATE { get; set; }
	public DateTime? UPDATED_DATE { get; set; }
	public long? ORG_ID { get; set; }
	public long? JOB_ID { get; set; }
	public int? LM { get; set; }
	public bool? ISOWNER { get; set; }
	public int? CSM { get; set; }
	public bool? IS_NONPHYSICAL { get; set; }
	public long? MASTER { get; set; }
	public int? CONCURRENT { get; set; }
	public bool? IS_PLAN { get; set; }
	public long? INTERIM { get; set; }
	public DateTime? EFFECTIVE_DATE { get; set; }
	public string? TYPE_ACTIVITIES { get; set; }
	public string? FILENAME { get; set; }
	public string? UPLOADFILE { get; set; }
	public int? WORKING_TIME { get; set; }

	public int? WORK_LOCATION { get; set; }
	public int? HIRING_STATUS { get; set; }
    public bool? IS_TDV { get; set; }
    public bool? IS_NOTOT { get; set; }

    
}
