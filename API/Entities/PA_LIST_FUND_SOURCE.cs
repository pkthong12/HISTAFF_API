using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("PA_LIST_FUND_SOURCE")]
    public class PA_LIST_FUND_SOURCE:BASE_ENTITY
    {
        public string CODE { get; set; } = null!;
        public string NAME { get; set;} = null!;
        public long? COMPANY_ID { get; set;}
        public bool? IS_ACTIVE { get; set; }
        public string? NOTE { get; set;}
        public string? CREATED_LOG { get; set;}
        public string? UPDATED_LOG { get; set;}

    }
}
