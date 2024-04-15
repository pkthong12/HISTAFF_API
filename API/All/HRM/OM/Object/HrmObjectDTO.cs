using API.Main;

namespace API.All.HRM.OM.Object
{
    public class HrmObjectDTO : BaseDTO
    {
        public string? Name { get; set; }
        public string? CreatedByUsername { get; set; }
        public string? UpdatedByUsername { get; set; }
    }
}
