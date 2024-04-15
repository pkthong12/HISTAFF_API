namespace ProfileDAL.Models
{
    /*
    [Table("HU_EMPLOYEE")]
    public class Employee : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string CODE { get; set; }

        [MaxLength(1000)]
        public string AVATAR { get; set; }
        [Required]
        [MaxLength(50)]
        public string FIRST_NAME { get; set; }
        [Required]
        [MaxLength(50)]
        public string LAST_NAME { get; set; }
        [Required]
        [MaxLength(100)]
        public string FULLNAME { get; set; }
        [ForeignKey("Organization")]
        public int ORG_ID { get; set; }
        public long? CONTRACT_ID { get; set; }
        public DateTime? CONTRACT_EXPIRED { get; set; } // ngày hết hạn hợp đồng
        public int? CONTRACT_TYPE_ID { get; set; } // loai hop dong
        [ForeignKey("Position")]
        public int? POSITION_ID { get; set; }
        public int? RESIDENT_ID { get; set; } // ĐỐI TƯỢNG CƯ TRÚ 
        public DateTime? JOIN_DATE { get; set; } // NGÀY VÀO CHÍNH THỨC
        [NotMapped]
        public long? DIRECT_MANAGER { get; set; }// người quản lý trực tiếp
        public long? LAST_WORKING_ID { get; set; } //QUyết định mới nhất
        // Sơ yếu lí lịch
        [MaxLength(150)]
        public string IMAGE { get; set; } // ảnh nhân viên

        // [ForeignKey("Employee_Manager")]
        public long? DIRECT_MANAGER_ID { get; set; }// người quản lý trực tiếp

        [ForeignKey("GENDER")]
        public int? GENDER_ID { get; set; } // giới tính
        public DateTime? BIRTH_DATE { get; set; }
        [MaxLength(20)]
        public string ID_NO { get; set; } // CMND/CCCD
        public DateTime? ID_DATE { get; set; } // ngày cấp
        [MaxLength(100)]
        public string ID_PLACE { get; set; } // nơi cấp

        [ForeignKey("RELIGION")]
        public int? RELIGION_ID { get; set; } // Tôn giáo
        [ForeignKey("NATIVE")]
        public int? NATIVE_ID { get; set; } // Dân tộc
        [ForeignKey("NATIONALITY")]
        public int? NATIONALITY_ID { get; set; } // Quốc tịch
        // thường trú
        [MaxLength(150)]
        public string ADDRESS { get; set; } // địa chỉ thường trú
        [MaxLength(150)]
        public string BIRTH_PLACE { get; set; }

        public int? WORK_STATUS_ID { get; set; } // trạng thái nhân viên

        [ForeignKey("Province")]
        public int? PROVINCE_ID { get; set; }
        [ForeignKey("District")]
        public int? DISTRICT_ID { get; set; }
        [ForeignKey("Ward")]
        public int? WARD_ID { get; set; }
        // Hiện tại trú
        [MaxLength(150)]
        public string CUR_ADDRESS { get; set; } // địa chỉ thường trú
        [ForeignKey("CurProvince")]
        public int? CUR_PROVINCE_ID { get; set; }
        [ForeignKey("CurDistrict")]
        public int? CUR_DISTRICT_ID { get; set; }
        [ForeignKey("CurWard")]
        public int? CUR_WARD_ID { get; set; }

        public DateTime? EFFECT_DATE { get; set; } // ngày hl QĐ
        public DateTime? TER_EFFECT_DATE { get; set; } // ngày nghỉ việc
        [MaxLength(50)]
        public string ITIME_CODE { get; set; } // mã chấm công
        public long? SALARY_TYPE_ID { get; set; } // Bảng lương
        [MaxLength(150)]
        public string TAX_CODE { get; set; } // Mã số thuế cá nhân

        // Thông tin phụ
        [MaxLength(50)]
        public string MOBILE_PHONE { get; set; }
        [MaxLength(50)]
        public string WORK_EMAIL { get; set; }
        [MaxLength(50)]
        public string EMAIL { get; set; }
        [ForeignKey("MARITAL_STATUS")]
        public int? MARITAL_STATUS_ID { get; set; } // Tình trạng hôn 
        [MaxLength(20)]
        public string PASS_NO { get; set; } // số hộ chiếu
        public DateTime? PASS_DATE { get; set; }
        public DateTime? PASS_EXPIRE { get; set; }
        [MaxLength(100)]
        public string PASS_PLACE { get; set; }
        [MaxLength(20)]
        public string VISA_NO { get; set; } // số visa
        public DateTime? VISA_DATE { get; set; }
        public DateTime? VISA_EXPIRE { get; set; }
        [MaxLength(100)]
        public string VISA_PLACE { get; set; }

        public int? SENIORITY { get; set; } // Thâm niên


        [MaxLength(20)]
        public string WORK_PERMIT { get; set; } // so giay phep lao dong
        public DateTime? WORK_PERMIT_DATE { get; set; }
        public DateTime? WORK_PERMIT_EXPIRE { get; set; }
        [MaxLength(150)]
        public string WORK_PERMIT_PLACE { get; set; }


        [MaxLength(150)]
        public string WORK_NO { get; set; } // so cchn
        public DateTime? WORK_DATE { get; set; }
        [MaxLength(150)]
        public string WORK_PLACE { get; set; }
        [MaxLength(250)]
        public string WORK_SCOPE { get; set; }

        [MaxLength(50)]
        public string CONTACT_PER { get; set; } // người liên hệ khi cần
        [MaxLength(20)]
        public string CONTACT_PER_PHONE { get; set; }
        // Tài khoản
        // [ForeignKey("bank")]
        public int? BANK_ID { get; set; }
        // Ngân hàng
        // [ForeignKey("bankBranch
        [MaxLength(500)]
        public string BANK_BRANCH { get; set; } // Chi nhánh Ngân hàng
        [MaxLength(50)]
        public string BANK_NO { get; set; } // Số tài 
        //Education
        [MaxLength(500)]
        public string SCHOOL_ID { get; set; } // Tên trường
        [MaxLength(500)]
        public string QUALIFICATION_ID { get; set; } // Trình độ chuyên môn
        public int? QUALIFICATIONID { get; set; } // Hình thức đào tạo
        [ForeignKey("TRAINING_FORM")]
        public int? TRAINING_FORM_ID { get; set; } // Hình thức đào tạo
        [ForeignKey("LEARNING_LEVEL")]
        public int? LEARNING_LEVEL_ID { get; set; } // Trình độ học vấn
        [MaxLength(50)]
        public string LANGUAGE { get; set; } //Ngoại ngữ
        [MaxLength(50)]
        public string LANGUAGE_MARK { get; set; } //Điểm số xếp loại
        public decimal? SAL_TOTAL { get; set; }
        public decimal? SAL_BASIC { get; set; }
        public decimal? SAL_RATE { get; set; }
        public decimal? DAY_OF { get; set; } // Số tài 
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Position Position { get; set; }
        // public Employee Employee_Manager { get; set; }
        public Ward Ward { get; set; }
        public Organization Organization { get; set; }
        public District District { get; set; }
        public Province Province { get; set; }
        public Ward CurWard { get; set; }
        public District CurDistrict { get; set; }
        public Province CurProvince { get; set; }
        public OtherList LEARNING_LEVEL { get; set; }
        public OtherList TRAINING_FORM { get; set; }
        public OtherList QUALIFICATION { get; set; }
        public OtherList SCHOOL { get; set; }
        public OtherList MARITAL_STATUS { get; set; }
        public OtherList GENDER { get; set; }
        public OtherList RELIGION { get; set; }
        public OtherList NATIVE { get; set; }
        public OtherList NATIONALITY { get; set; }
        
        public int? STAFF_RANK_ID { get; set; }
    };

    [Table("HU_FAMILY")]
    public class Situation : IAuditableEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public long TENANT_ID { get; set; }
        public int? RELATIONSHIP_ID { get; set; }
        [ForeignKey("Employee")]
        public long EMPLOYEE_ID { get; set; }
        public string NAME { get; set; }

        public string NO { get; set; } // cmnd
        public string TAX_NO { get; set; } // ma so thue
        public string FAMILY_NO { get; set; } // Số sổ hộ khẩu
        public string FAMILY_NAME { get; set; } // Tên chủ hộ
        public string ADDRESS { get; set; } // Địa chỉ thường chú

        public DateTime? BIRTH { get; set; }
        public DateTime? DATE_START { get; set; }
        public DateTime? DATE_END { get; set; }
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Employee Employee { get; set; }
    }
    [Table("HU_FAMILY_EDIT")]
    public class SituationEdit : IAuditableEntity
    {
        public long? ID { get; set; }
        
        [ForeignKey("employee")]
        public long? EMPLOYEE_ID { get; set; }
        public int? RELATIONSHIP_ID { get; set; }
        public string NAME { get; set; }
        public string NO { get; set; } // cmnd
        public string TAX_NO { get; set; } // ma so thue
        public string FAMILY_NO { get; set; } // Số sổ hộ khẩu
        public string FAMILY_NAME { get; set; } // Tên chủ hộ
        public string ADDRESS { get; set; } // Địa chỉ thường chú
        public DateTime? BIRTH { get; set; }
        public DateTime? DATE_START { get; set; }
        public DateTime? DATE_END { get; set; }
        public string EMPLOYEE_CODE { get; set; }
        public int? STATUS { get; set; } // Trạng thái
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Employee employee { get; set; }

    }

    [Table("HU_EMPLOYEE_TMP")]
    public class EmployeeTmp
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string CODE { get; set; }
        [MaxLength(150)]
        public string REF_CODE { get; set; }
        [Required]
        [MaxLength(30)]
        public string FIRST_NAME { get; set; }
        [Required]
        [MaxLength(30)]
        public string LAST_NAME { get; set; }
        public string FULLNAME { get; set; }
        public string ORG_NAME { get; set; }
        public int? ORG_ID { get; set; }
        public string POSITION { get; set; }
        public int? POS_ID { get; set; }
        public string RESIDENT { get; set; } // ĐỐI TƯỢNG CƯ TRÚ 
        public int? RESIDENT_ID { get; set; }
        public string GENDER { get; set; } // giới tính
        public int? GENDER_ID { get; set; }

        public string BIRTH_DATE { get; set; }
        public DateTime? BIRTH_DATE_INPUT { get; set; }
        [MaxLength(20)]
        public string ID_NO { get; set; } // CMND/CCCD
      //  public string ID_DATE { get; set; } // ngày cấp
        public DateTime? ID_DATE_INPUT { get; set; } // ngày cấp
        [MaxLength(100)]
        public string ID_PLACE { get; set; } // nơi cấp

        public string RELIGION { get; set; } // Tôn giáo
        public int? RELIGION_ID { get; set; } // Tôn giáo
        public string NATIVE { get; set; } // Dân tộc
        public int? NATIVE_ID { get; set; } // Dân tộc
        public string NATIONALITY { get; set; } // Quốc tịch
        public int? NATIONALITY_ID { get; set; } // Quốc tịch
        // thường trú
        [MaxLength(150)]
        public string ADDRESS { get; set; } // địa chỉ thường trú
        [MaxLength(150)]
        public string BIRTH_PLACE { get; set; }

        public string PROVINCE { get; set; }
        public int? PROVINCE_ID { get; set; }
        public string DISTRICT { get; set; }
        public int? DISTRICT_ID { get; set; }
        public string WARD { get; set; }
        public int? WARD_ID { get; set; }
        // Hiện tại trú
        [MaxLength(150)]
        public string CUR_ADDRESS { get; set; } // địa chỉ thường trú
        public string CUR_PROVINCE { get; set; }
        public int? CUR_PROVINCE_ID { get; set; }
        public string CUR_DISTRICT { get; set; }
        public int? CUR_DISTRICT_ID { get; set; }
        public string CUR_WARD { get; set; }
        public int? CUR_WARD_ID { get; set; }

        [MaxLength(50)]
        public string ITIME_CODE { get; set; } // mã chấm công
        [MaxLength(150)]
        public string TAX_CODE { get; set; } // Mã số thuế cá nhân
        public string CONTACT_PER { get; set; }
        public string CONTACT_PER_PHONE { get; set; }

        // Thông tin phụ
        [MaxLength(50)]
        public string MOBILE_PHONE { get; set; }
        [MaxLength(50)]
        public string WORK_EMAIL { get; set; }
        [MaxLength(50)]
        public string EMAIL { get; set; }
        public string MARITAL_STATUS { get; set; } // Tình trạng hôn 
        public int? MARITAL_STATUS_ID { get; set; } // Tình trạng hôn 

        public string PASS_NO { get; set; } // số hộ chiếu
        //public string PASS_DATE { get; set; }
        public DateTime? PASS_DATE_INPUT { get; set; }
        //public string PASS_EXPIRE { get; set; }
        public DateTime? PASS_EXPIRE_INPUT { get; set; }
        [MaxLength(100)]
        public string PASS_PLACE { get; set; }
        [MaxLength(20)]
        public string VISA_NO { get; set; } // số visa
        //public string VISA_DATE { get; set; }
        public DateTime? VISA_DATE_INPUT { get; set; }
        //public string VISA_EXPIRE { get; set; }
        public DateTime? VISA_EXPIRE_INPUT { get; set; }
        [MaxLength(100)]
        public string VISA_PLACE { get; set; }
        [MaxLength(20)]
        public string WORK_PERMIT { get; set; } // so giay phep lao dong
        //public string WORK_PERMIT_DATE { get; set; }
        public DateTime? WORK_PERMIT_DATE_INPUT { get; set; }
        //public string WORK_PERMIT_EXPIRE { get; set; }
        public DateTime? WORK_PERMIT_EXPIRE_INPUT { get; set; }
        [MaxLength(50)]
        public string WORK_PERMIT_PLACE { get; set; }
        [MaxLength(250)]
        public string BANK_NAME { get; set; }
        [MaxLength(500)]
        public string BANK_BRANCH { get; set; } // Chi nhánh Ngân hàng
        [MaxLength(50)]
        public string BANK_NO { get; set; } // Số tài 
        public int? BANK_ID { get; set; } // Số tài 

        public string SCHOOL_ID { get; set; } // Tên trường
        public int? EMP_ID { get; set; } // Tên trường
        [MaxLength(500)]
        public string QUALIFICATION_ID { get; set; } // Trình độ chuyên môn
        public string TRAINING_FORM { get; set; } // Hình thức đào tạo
        public int? TRAINING_FORM_ID { get; set; } // Hình thức đào tạo
        public string LEARNING_LEVEL { get; set; } // Trình độ học vấn
        public int? LEARNING_LEVEL_ID { get; set; } // Trình độ học vấn
                                                      // thường trú
        [MaxLength(150)]
        public string LANGUAGE { get; set; } // địa chỉ thường trú
    }
    [Table("HU_EMPLOYEE_PAPERS")]
    public class EmployeePapers : IAuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        public int PAPER_ID { get; set; }
        public int EMP_ID { get; set; }
        public DateTime DATE_INPUT { get; set; }
        [MaxLength(450)]
        public string URL { get; set; }
        [MaxLength(550)]
        public string NOTE { get; set; }
        public bool STATUS_ID { get; set; }// 1:miễn nộp
        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }

    [Table("HU_EMPLOYEE_EDIT")]
    public class EmployeeEdit: IAuditableEntity
    {
        public long? Id { get; set; }
        
        [ForeignKey("employee")]
        public long EMPLOYEE_ID { get; set; }
        public string CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string FULLNAME { get; set; }
        public int? ORG_ID { get; set; }
        public int? CONTRACT_ID { get; set; }
        public int? CONTRACT_TYPE_ID { get; set; }
        public int? POSITION_ID { get; set; }

        public int? DIRECT_MANAGER { get; set; }
        public int? LAST_WORKING_ID { get; set; }
        public int? DIRECT_MANAGER_ID { get; set; }
        public int? GENDER_ID { get; set; }

        public string ID_NO { get; set; } // CMND/CCCD
        public string ID_PLACE { get; set; } // nơi cấp

        public int? RELIGION_ID { get; set; }
        public int? NATIVE_ID { get; set; }
        public int? NATIONALITY_ID { get; set; }

        public string ADDRESS { get; set; } 
        public string BIRTH_PLACE { get; set; }

        public int? WORK_STATUS_ID { get; set; }
        public int? PROVINCE_ID { get; set; }
        public int? DISTRICT_ID { get; set; }
        public int? WARD_ID { get; set; }
        public DateTime? TER_EFFECT_DATE { get; set; }
        public string ITIME_CODE { get; set; }
        public DateTime? JOIN_DATE { get; set; }
        public long? SALARY_TYPE_ID { get; set; }
        public string TAX_CODE { get; set; }
        public string MOBILE_PHONE { get; set; }
        public string WORK_EMAIL { get; set; }
        public string EMAIL { get; set; }
        public int? MARITAL_STATUS_ID { get; set; }
        public string PASS_NO { get; set; }
        public DateTime? PASS_DATE { get; set; }
        public DateTime? PASS_EXPIRE { get; set; }
        public string PASS_PLACE { get; set; }
        public string VISA_NO { get; set; }
        public DateTime? VISA_DATE { get; set; }
        public DateTime? VISA_EXPIRE { get; set; }
        public string VISA_PLACE { get; set; }
        public int? SENIORITY { get; set; }
        public int? STATUS { get; set; }
        public DateTime? BIRTH_DATE { get; set; }

        //Giấy phép lao động
        public string WORK_PERMIT { get; set; }
        public DateTime? WORK_PERMIT_DATE { get; set; }
        public DateTime? WORK_PERMIT_EXPIRE { get; set; }
        public string WORK_PERMIT_PLACE { get; set; }


        //Chứng chỉ hành nghề
        public string WORK_NO { get; set; }
        public DateTime? WORK_DATE { get; set; }
        public string WORK_SCOPE{ get; set; }
        public string WORK_PLACE { get; set; }

        // người liên hệ khi cần
        public string CONTACT_PER { get; set; }
        public string CONTACT_PER_PHONE { get; set; }

        //Tài khoản
        public int? BANK_ID { get; set; }
        public string BANK_BRANCH { get; set; }
        public string BANK_NO { get; set; }

        //Trình độ học vấn
        public string SCHOOL_ID { get; set; }
        public string SCHOOLNAME { get; set; }
        public string TRAININGFORMNAME { get; set; }
        public string LEARNINGLEVELNAME { get; set; }
        public string QUALIFICATION_ID { get; set; }
        public int? QUALIFICATIONID { get; set; }

        public string LANGUAGE_MARK { get; set; }
        public int? TRAINING_FORM_ID { get; set; }
        public int? LEARNING_LEVEL_ID { get; set; }
        public string LANGUAGE { get; set; }
        public int? RESIDENT_ID { get; set; }
        public int? SAL_TOTAL { get; set; }
        public DateTime? ID_DATE { get; set; } // ngày cấp

        // tạm trú
        public string CUR_ADDRESS { get; set; }
        public int? CUR_WARD_ID { get; set; }
        public int? CUR_DISTRICT_ID { get; set; }
        public int? CUR_PROVINCE_ID { get; set; }

        [MaxLength(450)]
        public string CREATED_BY { get; set; }
        [MaxLength(450)]
        public string UPDATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public Employee employee { get; set; }

        [ForeignKey("JobBand")]
        public int? STAFF_RANK_ID { get; set; }
        
    }
    */
}
