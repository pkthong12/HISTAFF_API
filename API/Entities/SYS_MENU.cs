using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("SYS_MENU")]
public partial class SYS_MENU: BASE_ENTITY
{

    public string CODE { get; set; } = null!;

    public bool? ROOT_ONLY { get; set; }

    public int? ORDER_NUMBER { get; set; }

    public string? ICON_FONT_FAMILY { get; set; }

    public string? ICON_CLASS { get; set; }

    public string? SYS_MENU_SERVICE_METHOD { get; set; }

    public string? URL { get; set; }

    public long? PARENT { get; set; }

    public long? FUNCTION_ID { get; set; }

    public bool? INACTIVE { get; set; }

}
