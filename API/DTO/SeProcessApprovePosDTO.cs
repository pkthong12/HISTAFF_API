using API.Main;

namespace API.DTO
{
    public class SeProcessApprovePosDTO : BaseDTO
    {
        public long? ProcessApproveId { get; set; }
        //public long? ProcessId { get; set; }
        public long? PosId { get; set; }
        public bool? IsDirectMngOfDirectMng { get; set; }
        public bool? IsMngAffiliatedDepartments { get; set; }
        public bool? IsMngSuperiorDepartments { get; set; }
        public bool? IsDirectManager { get; set; }
    }
}
