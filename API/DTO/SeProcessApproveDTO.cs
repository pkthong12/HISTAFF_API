using API.Main;

namespace API.DTO
{
    public class SeProcessApproveDTO : BaseDTO
    {
        //public long? OrgId { get; set; }//SDTC
        public string? ApprovalLevelName { get; set; }//Ten cap phe duyet
        public int? LevelOrderId { get; set; }//Thu tu cap
        public bool? ApprovalPosition { get; set; }//Bo qua neu vi tri phe duyet trong
        public bool? SameApprover { get; set; }//Bo qua neu trung nguoi phe duyet
        public long? ProcessId { get; set; }//Quy trinh
        public string? ProcessName { get; set; }
        public bool? DirectManager { get; set; }//Quan li truc tiep
        public bool? ManagerAffiliatedDepartments { get; set; }//Quan ly phong ban truc thuoc
        public bool? ManagerSuperiorDepartments { get; set; }//Quan ly phong ban cap tren
        public bool? IsDirectMngOfDirectMng { get; set; } //Quan ly truc tiep cua quan ly truc tiep
        public string? PosName { get; set; }
        public int? ListCheck { get; set; }//tu dong lua chon nguoi phe duyet 
        public List<long>? CheckList { get; set; }
        public List<long>? PosIds { get; set; }//list chuc danh
        public string? Approve { get; set; }//Phe duyet
        public string? Refuse { get; set; }//Tu choi
        public string? ChooseAnApprover { get; set; }//Lua chon nguoi phe duyet

        public string? UpdatedLog { get; set; }
        public string? CreatedLog { get; set; }
    }
}
