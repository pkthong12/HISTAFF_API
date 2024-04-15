using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Main;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

namespace API.Entities;
[Table("HU_COMPANY")]
public partial class HU_COMPANY : BASE_ENTITY
{
	// anh Nguyễn Văn Tân bảo xóa cái này đi, [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public string? NAME_VN { get; set; }
	public string? NAME_EN { get; set; }
	public long? ORG_ID { get; set; }
	public string? GPKD_ADDRESS { get; set; }
	public long? REGION_ID { get; set; }
	public string? PHONE_NUMBER { get; set; }
	public string? WORK_ADDRESS { get; set; }
	public long? INS_UNIT { get; set; }
	public long? PROVINCE_ID { get; set; }
	public long? DISTRICT_ID { get; set; }
	public long? WARD_ID { get; set; }
	public string? FILE_LOGO { get; set; }
	public string? BANK_ACCOUNT { get; set; }
	public long? BANK_ID { get; set; }
	public long? BANK_BRANCH_ID { get; set; }
	public string? FILE_HEADER { get; set; }
	public string? PIT_CODE { get; set; }
	public string? PIT_CODE_CHANGE { get; set; }
	public DateTime? PIT_CODE_DATE { get; set; }
	public string? FILE_FOOTER { get; set; }
	public long? REPRESENTATIVE_ID { get; set; }
	public long? SIGN_ID { get; set; }
	public string? PIT_CODE_PLACE { get; set; }
	public string? GPKD_NO { get; set; }
	public DateTime? GPKD_DATE { get; set; }
	public string? WEBSITE { get; set; }
	public string? FAX { get; set; }
	public string? NOTE { get; set; }
	public bool IS_ACTIVE { get; set; }
	public string? BANK_BRANCH { get; set; }
	public string? CODE { get; set; }
	public int? ORDER { get; set; }
	public string? SHORT_NAME { get; set; }
	public string? EMAIL { get; set; }
}
