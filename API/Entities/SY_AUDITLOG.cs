using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SY_AUDITLOG")]
public partial class SY_AUDITLOG
{
    [Key]
    public long AUDITLOG_ID { get; set; }

    public string? USER_ID { get; set; }

    public DateTime EVENT_DATE { get; set; }

    public string? EVENT_TYPE { get; set; }

    public string? TABLE_NAME { get; set; }

    public string? RECORD_ID { get; set; }

    public string? COLUMN_NAME { get; set; }

    public string? ORIGINAL_VALUE { get; set; }

    public string? NEW_VALUE { get; set; }
}

public interface IDescribableEntity
{
    // Override this method to provide a description of the entity for audit purposes
    string Describe();
}