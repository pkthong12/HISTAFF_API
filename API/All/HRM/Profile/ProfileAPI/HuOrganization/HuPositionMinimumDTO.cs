namespace API.All.HRM.Profile.ProfileAPI.HuOrganization
{
    public class HuPositionMinimumDTO
    {
        [JsonProperty(Order = 1)]
        public long? Id { get; set; }
        [JsonProperty(Order = 2)]
        public string? Code { get; set; }
        [JsonProperty(Order = 3)]
        public string? Name { get; set; }
        [JsonProperty(Order = 4)]
        public long? Master { get; set; }
        [JsonProperty(Order = 5)]
        public string? MasterFullname { get; set; }
        [JsonProperty(Order = 6)]
        public string? MasterAvatar { get; set; }
        [JsonProperty(Order = 7)]
        public long? Interim { get; set; }
        [JsonProperty(Order = 8)]
        public string? InterimFullname { get; set; }
        [JsonProperty(Order = 9)]
        public string? InterimAvatar { get; set; }

        [JsonProperty(Order = 10)]
        public long? OrgId{ get; set; }
        [JsonProperty(Order = 11)]
        public bool? IsTdv { get; set; } // là trưởng đơn vị
        public long? TdvId { get; set; }
        [JsonProperty(Order = 12)]
        public string? TdvFullname { get; set; }
        [JsonProperty(Order = 13)]
        public string? TdvAvatar { get; set; }

        public string? JobName { get; set; }
        public string? PositionName { get; set; }
    }
}
