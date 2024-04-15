namespace API.All.HRM.Profile.ProfileAPI.HuOrganization
{
    public class HuOrganizationMinimumDTO
    {
        [JsonProperty(Order = 1)]
        public long Id { get; set; }

        [JsonProperty(Order = 2)]
        public required string Name { get; set; }

        [JsonProperty(Order = 3)]
        public long? ParentId { get; set; }

        [JsonProperty(Order = 4)]
        public int OrderNum { get; set; }

        // if Protected, user will only see the item, but no selection allow on CoreOrgTree
        [JsonProperty(Order = 5)]
        public bool? Protected { get; set; }

        [JsonProperty(Order = 6)]
        public int? Level { get; set; }

        [JsonProperty(Order = 7)]
        public string Code { get; set; } = null!;

        [JsonProperty(Order = 8)]
        public bool IsActive { get; set; }

    }
}
