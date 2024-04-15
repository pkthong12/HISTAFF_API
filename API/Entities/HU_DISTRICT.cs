using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_DISTRICT")]
public partial class HU_DISTRICT
{
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long ID { get; set; }
	public string? CODE { get; set; }
	public long PROVINCE_ID { get; set; }

	public string? NAME { get; set; }

	public bool IS_ACTIVE { get; set; }

	public string? NOTE { get; set; }

	public DateTime? CREATED_DATE { get; set; }
	public string? CREATED_BY { get; set; }
	public DateTime? UPDATED_DATE { get; set; }
	public string? UPDATED_BY { get; set; }

}
