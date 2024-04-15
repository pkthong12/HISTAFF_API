using Common.Paging;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{


    public class QuestionDTO
    {
        [Required(ErrorMessage = "{0}_Required")]
        public string Name { get; set; }
        public DateTime? Expire { get; set; }
        public bool? IsMultiple { get; set; }
        public bool? IsAddAnswer { get; set; }
        public List<AnswerDTO> Answers { get; set; }
    }
    public class QuestionPagingDTO : Pagings
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public DateTime? Expire { get; set; }
        public bool? IsMultiple { get; set; }
        public bool? IsAddAnswer { get; set; }
        public bool? IsActive { get; set; }
        public List<Result> Results { get; set; }
    }
    public class Result
    {
        public string Answer { get; set; }
        public double Vote { get; set; }
    }
    public class AnswerDTO
    {
        public string Answer { get; set; }
    }
    public class QuestionOutputDTO
    {
        [Required(ErrorMessage = "{0}_Required")]
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? Expire { get; set; }
        public bool? IsMultiple { get; set; }
        public bool? IsAddAnswer { get; set; }
        public int Counter { get; set; }

        public List<AnswerOutputDTO> Answers { get; set; }

    }
    public class AnswerOutputDTO
    {
        public long? Id { get; set; }
        public string Answer { get; set; }
        public bool IsVote { get; set; }
        public List<EmployeeOutputDTO> Employees { get; set; }
    }
    public class EmployeeOutputDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
    public class PortalVoteParam
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? Expire { get; set; }
    }
}
