using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Main;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace API.Entities;
[Table("INS_ARISING")]

public partial class INS_ARISING : BASE_ENTITY
{
    public long? INS_GROUP_TYPE { get; set; }

    public long? INS_TYPE_ID { get; set; }

    public long? INS_TYPE_CHOOSE { get; set; }

    public long? PKEY_REF { get; set; }

    public string? TABLE_REF { get; set; }

    public DateTime? EFFECT_DATE { get; set; }

    public long? EMPLOYEE_ID { get; set; }

    public long? OLD_ORG_ID { get; set; }

    public long? NEW_ORG_ID { get; set; }

    public double? OLD_SAL {  get; set; }

    public double? NEW_SAL { get; set; }

    public long? OLD_POSITION_ID { get; set; }

    public long? NEW_POSITION_ID { get; set; }

    public bool? SI { get; set; }

    public bool? HI { get; set; }

    public bool? UI { get; set; }

    public bool? AI { get; set; }

    public string? REASONS { get; set; }

    public long? STATUS { get; set; }

    public long? INS_ORG_ID { get; set; }

    public DateTime? DECLARED_DATE { get; set; }

    public bool? IS_DELETED { get; set; }
    public long? INS_INFORMATION_ID { get; set; }

    public long? INS_SPECIFIED_ID {  get; set; }
}
