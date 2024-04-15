using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("HU_ANSWER")]
public partial class HU_ANSWER: BASE_ENTITY
{
    public string? ANSWER { get; set; }

    public long QUESTION_ID { get; set; }

    public bool? IS_ACTIVE { get; set; }

    //public virtual ICollection<HU_ANSWER_USER> HU_ANSWER_USERs { get; set; } = new List<HU_ANSWER_USER>();
    
    //[InverseProperty("HU_ANSWERs")]
    //public virtual HU_QUESTION QUESTION { get; set; } = null!;
}
