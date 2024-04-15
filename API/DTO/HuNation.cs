using API.Main;

namespace API.DTO
{
    public class HuNationDTO : BaseDTO
    {
        
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public string? IsActiveStr { get; set; }
        public bool? IsActive { get; set; }
    }
}
