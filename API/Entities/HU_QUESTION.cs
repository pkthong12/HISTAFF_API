using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_QUESTION")]
public partial class HU_QUESTION: BASE_ENTITY
{
    public string? NAME { get; set; }

    public DateTime? EXPIRE { get; set; }

    public bool IS_MULTIPLE { get; set; }

    public bool IS_ADD_ANSWER { get; set; }

    public bool? IS_ACTIVE { get; set; }

    //public virtual ICollection<HU_ANSWER> HU_ANSWERs { get; set; } = new List<HU_ANSWER>();
}
