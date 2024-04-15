using Common.Paging;
namespace CoreDAL.Models
{
    public class SysUserDTO :  Pagings
    {
        public string Id { get; set; }
        public long? GroupId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string GroupName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Password { get; set; }
        public bool? Lock { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class SysUserInputDTO
    {
        public string Id { get; set; }
        public int GroupId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Password { get; set; }
    }
}
