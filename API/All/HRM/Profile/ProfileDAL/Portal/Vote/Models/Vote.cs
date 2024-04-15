namespace ProfileDAL.Models
{
    /*
    [Table("HU_QUESTION")]
    public class Question : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string NAME { get; set; }
        public DateTime? EXPIRE { get; set; }
        [DefaultValue("1")]
        public Boolean IS_MULTIPLE { get; set; }// cho phep chọn nhiều
        [DefaultValue("1")]
        public Boolean IS_ADD_ANSWER { get; set; } // cho phép thêm câu trả lời
        
        [DefaultValue("1")]
        public bool? IS_ACTIVE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

    }
    [Table("HU_ANSWER")]
    public class Answer : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string ANSWER { get; set; }
        [ForeignKey("Question")]
        public long QUESTION_ID { get; set; }
        public Question Question { get; set; }
        
        [DefaultValue("1")]
        public bool? IS_ACTIVE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

    }
    [Table("HU_ANSWER_USER")]
    public class AnswerUser : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey("Answer")]
        public long ANSWER_ID { get; set; }

        [ForeignKey("Employee")]
        public long EMP_ID { get; set; }

        public Answer Answer { get; set; }
        public Employee Employee { get; set; }
        
        [DefaultValue("1")]
        public bool? IS_ACTIVE { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

    }
    */
}
