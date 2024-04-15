using API.Main;

namespace API.DTO
{
    public class HuQuestionDTO: BaseDTO
    {
        public string? Name { get; set; }
        public DateTime? Expire { get; set; }
        public bool? IsMultiple { get; set; }
        public bool? IsAddAnswer { get; set; }
        public bool? IsActive { get; set; }

        public List<HuAnswerDTO>? Answers { get; set; }
    }
}
