using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("TR_COURSE")]
    public partial class TR_COURSE : BASE_ENTITY
    {
        public string? COURSE_CODE { get; set; }
        public string? COURSE_NAME { get;set; }
        public int? COURSE_DATE { get; set; }
        public decimal? COSTS { get; set; }
        public string? NOTE { get; set; }
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
        public bool? IS_ACTIVE { get; set; }

    }
}
