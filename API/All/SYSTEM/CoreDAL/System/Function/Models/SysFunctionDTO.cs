using Common.Paging;

namespace CoreDAL.ViewModels
{
    public class SysFunctionDTO : Pagings
    {
        public long? Id { get; set; }
        public long? GroupId { get; set; }
        public long? ModuleId { get; set; }

        public string GroupName { get; set; }
        public string ModuleName { get; set; }

        public long AppId { get; set; }
        public string AppName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        //public string? Link { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<long>? Actions { get; set; }
    }

    public class SysFunctionInputDTO
    {
        public long? Id { get; set; }
        public long GroupId { get; set; }
        public long ModuleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
        public List<long>? Actions { get; set; }
    }

    public class SysActionDTO
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? NameVN { get; set; }
        public string? NameEn { get; set; }
    }
    public class SysFunctionActionDTO
    {
        public long ActionId { get; set; }
        public string? ActionCode { get; set; }
        public string? ActionNameCode { get; set; }
        public bool? Allowed { get; set; }
    }
}
