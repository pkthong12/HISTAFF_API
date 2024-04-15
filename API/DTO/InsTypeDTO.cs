using API.Main;

namespace API.DTO
{
    public class InsTypeDTO: BaseDTO
    {
        public string? Code { get; set; }
        public long? TypeId { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public string? TypeName { get; set; }
    }
}
