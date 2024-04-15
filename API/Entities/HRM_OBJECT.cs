using API.Main;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HRM_OBJECT")]
    public class HRM_OBJECT: BASE_ENTITY
    {
        public string NAME { get; set; }
    }
}
