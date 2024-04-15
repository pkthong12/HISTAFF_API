namespace API.All.HRM.Profile.ProfileAPI.HuOrganization
{
    public class HuOrganizationMinimumWithPositionDTO: HuOrganizationMinimumDTO
    {
        // As a rule, each OrgItem must not have any Position with more than one TDV, Master employee.
        // But for letting the query to return a response, we use List<>
        // And will check bussiness error on FE
        public List<HuPositionMinimumDTO>? Tdvs { get; set; } // trưởng đơn vị
        public List<HuPositionMinimumDTO>? MasterPositions { get; set; } // Đương nhiệm
        public List<HuPositionMinimumDTO>? InterimPositions { get; set; } // Kế nhiệm
    }
}
