using API.DTO;

namespace API.All.HRM.Profile.ProfileAPI.HuOrganization
{
    public class HuOrganizationTreeBlockPositionDTO: HuOrganizationMinimumDTO
    {
        [JsonProperty(Order = 5)]
        public required List<HuOrganizationTreeBlockPositionDTO> Children { get; set; }

        [JsonProperty(Order = 6)]
        public required List<HuPositionMinimumDTO> Positions { get; set; }
    }
}
