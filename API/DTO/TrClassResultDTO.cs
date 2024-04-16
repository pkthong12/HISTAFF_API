using API.Main;

namespace API.DTO
{
    public class TrClassResultDTO : BaseDTO
    {
        public long? TrClassId { get; set; }
        public long? EmployeeId { get; set; }
        public long? Grade { get; set; }
        public string? Remark { get; set; }
        public string? CreatedLog { get; set; }
        public string? UpdatedLog { get; set; }
    }
}
