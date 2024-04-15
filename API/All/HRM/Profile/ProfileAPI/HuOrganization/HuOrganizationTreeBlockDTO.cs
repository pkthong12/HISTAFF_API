namespace API.All.HRM.Profile.ProfileAPI.HuOrganization
{
    public class HuOrganizationTreeBlockDTO: HuOrganizationMinimumDTO
    {
        [JsonProperty(Order = 5)]
        public required List<HuOrganizationTreeBlockDTO> Children { get; set; }
    }
}
