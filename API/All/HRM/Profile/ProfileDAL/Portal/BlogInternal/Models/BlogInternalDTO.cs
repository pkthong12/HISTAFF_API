using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class BlogInternalDTO : Pagings
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool? IsActive { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class BlogInternalInputDTO
    {
        public long? Id { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "{0}_Required")]
        public string Content { get; set; }
        public int ThemeId { get; set; }
    }


    public class BlogPortalDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int? ThemeId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Img { get; set; }
    }
    public class BlogNotifi
    {
        public long Id { get; set; }
        public long NotyId { get; set; }
        public string Title { get; set; }
        public long Type { get; set; }
        public long Status { get; set; }
        public string Img { get; set; }
        public int IsRead { get; set; }
        public string LstIsRead { get; set; }
        public string FullName { get; set; }
        public string LstEmpNoti { get; set; }
        public string EmpNotifyId { get; set; }
        public DateTime? Date { get; set; }
    }
    public class NotifyView
    {
        public decimal Id { get; set; }
        public decimal NotyId { get; set; }
        public string Title { get; set; }
        public decimal Type { get; set; }
        public decimal Status { get; set; }
        public string Img { get; set; }
        public decimal IsRead { get; set; }
        public string LstIsRead { get; set; }
        public string FullName { get; set; }
        public string LstEmpNoti { get; set; }
        public string EmpNotifyId { get; set; }
        public string Date { get; set; }
    }

    public class NotifyApproveView
    {
        public decimal TotalOt { get; set; }
        public decimal TotalReg { get; set; }
        public decimal TotalLate { get; set; }
    }
}
