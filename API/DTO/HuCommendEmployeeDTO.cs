using API.Main;

namespace API.DTO
{
    public class HuCommendEmployeeDTO:BaseDTO
    {
        public long? EmployeeId { get; set; }
        public long? CommendId { get; set; }
        public bool? IsActive { get; set; }
        public long? StatusId { get; set; }
    }
}
