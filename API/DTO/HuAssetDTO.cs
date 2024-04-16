using API.Main;

namespace API.DTO
{
    public class HuAssetDTO : BaseDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public long? GroupAssetId { get; set; }
        public string? GroupAssetName { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? Status { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
    }
}
