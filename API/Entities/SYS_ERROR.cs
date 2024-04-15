using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("SYS_ERROR")]
    public class SYS_ERROR
    {
        public long ID { get; set; }
        public string ERROR_CODE { get; set; }
        public string PRIMARY { get; set; }
        public string SECONDARY { get; set; }

    }
}
