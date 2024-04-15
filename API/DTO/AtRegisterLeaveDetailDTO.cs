using CORE.Services.File;
using ProfileDAL.ViewModels;

namespace API.DTO
{
    public class AtRegisterLeaveDetailDTO
    {
        public long Id { get; set; }
        public long RegisterId { get; set; }
        public DateTime LeaveDate { get; set; }
      
    }
}
