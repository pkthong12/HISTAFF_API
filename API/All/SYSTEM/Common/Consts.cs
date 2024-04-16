namespace Common.Extensions
{
    public class Consts
    {
        public const int PAGE_SIZE = 20;
        public const int TRIAL = 7;
        public const int APPLICATION_SYSTEM = 1;
        public const string STATUS_CODE_400 = "400";
        public const string APP_NOT_EXISTS = "APPLICATION_NOT_EXISTS";
        public const string CODE_EXISTS = "CODE_EXISTS";
        public const string PACKAGE_NOT_EXISTS = "PACKAGE_NOT_EXISTS";
        public const string MODULE_NOT_EXISTS = "MODULE_NOT_EXISTS";
        public const string GROUP_FUNC_NOT_EXISTS = "GROUP_FUNC_NOT_EXISTS";
        public const string ID_NOT_FOUND = "ID_NOT_FOUND";


        public const int REGISTER_OFF = 2;
        public const int REGISTER_OT = 1;
        public const int REGISTER_LATE = 3;
        public const int REGISTER_ROOM = 4;
        public const int REGISTER_NEW = 0;
        public const int ACTION_CREATE = 1; // TẠO
        public const int ACTION_APPROVE = 2; // PHÊ DUYỆT
        public const int ACTION_DENIED = 3; // TỪ CHỐI

        public const string ALLOWANCE = "PC"; // code nhóm element phụ cấp
        public const string DATA_KPI = "DATA_KPI"; // code nhóm element phụ cấp

    }

    public class SysERP
    {
        public const string HR = "GOHR";
        public const int GoHR = 1;
    }

    public class SystemConfig
    {

        public const String DECISION_STATUS = "DECISION_STATUS";
        public const String STATUS = "STATUS";
        public const string OTHER_AREA = "AREAS";
        public const string PA_ELEMENT_GROUP = "PA_ELEMENT_GROUP";

        public const string APPLICATION = "APPLICATION";
        public const string ALLOWANCE_TYPE = "ALLOWANCE_TYPE";
        public const string SERVER = "SERVER";
        public const string STATUS_EMPLOYEE = "STATUS_EMPLOYEE";
        public const string STATUS_APPROVE = "STATUSAPPROVE";
        public const string STATUS_BHXH = "STATUS_BHXH";
        public const string OBJECT_COMMEND = "OBJECT_COMMEND";// Đối tượng khen thưởng xử phạt
        public const string SOURCE_COST = "SOURCE_COST";//Nguồn chi

        public const string SETTING_REMIND = "SETTING_REMIND";// thiet lap loi nhac
        public const string EMP_EXPIRE_CONTRACT = "EMP_EXPIRE_CONTRACT";// TNhan vien het han hop dong
        public const string EMP_BIRTH_DAY = "EMP_BIRTH_DAY";// Nhan vien sap den sinh nhat
        public const string EMP_REGISTER_CONTRACT = "EMP_REGISTER_CONTRACT";// Nhan vien chua lam hop dong
        public const string EMP_REGISTER_WORKING = "EMP_REGISTER_WORKING";// Nhan vien chua lam quyet dinh
        public const string EMP_METTTING_ROOM = "EMP_METTTING_ROOM";// Đặt phòng hợp
        public const string EMP_PAPER = "EMP_PAPER";// Thiếu giấy tờ cần nôpj
        public const string RESIDENT = "RESIDENT";// Đối tượng thường trú
        public const string PA_3P_EXCHANGE = "PA_3P_EXCHANGE";// quy đổi bảng lương 3p
        public const string LEARNING_LEVEL = "LEARNING_LEVEL";// quy đổi bảng lương 3p
        public const string PAPER = "PAPER";// quy đổi bảng lương 3p
        public const string NO_IMG = "http://gohr.vn:6868/upload/gohr/profile/no-img_100002130320211.png";
        //thiet lap thong bao
        public const string WARNING_TYPE = "WARNING_TYPE";// thiet lap chuong thong bao
        public const string WARN01= "WARN01"; //Nhân viên hết hạn hợp đồng thử việc
        public const string WARN02= "WARN02"; //Nhân viên hết hạn hợp đồng chính thức
        public const string WARN03= "WARN03"; //Nhân viên sắp đến sinh nhật
        public const string WARN04= "WARN04"; //Nhân viên chưa nộp đủ giấy tờ khi tiếp nhận
        public const string WARN05= "WARN05"; //Nhân viên nghỉ việc trong tháng
        public const string WARN06= "WARN06"; //Có bảo hiểm tạo mới
        public const string WARN07= "WARN07"; //Nhân viên nghỉ thai sản sắp đi làm lại
        public const string WARN08= "WARN08"; //Nhân viên đến tuổi nghỉ hưu
        public const string WARN09= "WARN09"; //Nhân viên hết hạn chứng chỉ
        public const string WARN10= "WARN10"; //Nhân viên hết kiêm nhiệm
        public const string WARN11= "WARN11"; //Nhân viên có người thân hết giảm trừ gia cảnh
        public const string WARN12= "WARN12"; //Lãnh đạo quản lý nghỉ hưu
        public const string WARN13= "WARN13"; //Lãnh đạo bổ nhiệm
        public const string WARN14= "WARN14"; //Nhân viên bổ nhiệm
        public const string WARN15= "WARN15"; //Nâng lương
        public const string WARN16= "WARN16"; //Khong hoan thanh nhiem vu
        public const string WARN17 = "WARN17"; // Nhân viên thay đổi thông tin sơ yếu Portal
        public const string WARN18 = "WARN18"; // Nhân viên thay đổi thông tin người thâm Portal
        public const string WARN19 = "WARN19"; // Trình độ học vấn
        public const string WARN20 = "WARN20"; // Bằng cấp - Chứng chỉ
        public const string WARN21 = "WARN21"; // NV sắp hết hạn Quyết định Điều động/biệt phái
        public const string WARN22 = "WARN22"; // Điều chỉnh Hồ sơ nhân viên
        public const string WARN23 = "WARN23"; // Điều chỉnh Thông tin người thân
        public const string WARN24 = "WARN24"; // Điều chỉnh Bằng cấp - Chứng chỉ
        public const string WARN25 = "WARN25"; // Yêu cầu điều chỉnh quá trình công tác
        public const string WARN26 = "WARN26"; // Yêu cầu điều chỉnh quá trình công tác trước đây
        public const string WARN27 = "WARN27"; // Nhân viên hết hạn phụ cấp 

    }

    public class OtherListConst
    {
        public const string GENDER = "GENDER";
        public const string RELATION = "RELATION"; // quan hệ nhân thân
        public const string FAMILY_STATUS = "FAMILY_STATUS"; //Tình trạng gia đình
        public const string MAJOR = "MAJOR"; //Trình độ chuyên môn
        public const string GRADUATE_SCHOOL = "GRADUATE_SCHOOL"; //Trường đào tạo
        public const string LEARNING_LEVEL = "LEARNING_LEVEL"; //Trình độ học vấn
        public const string TRAINING_FORM = "TRAINING_FORM"; //Hình thức đào tạo
        public const string RULE_SPACING = "RULE_SPACING"; // nguyên tắc giãn 3p
        public const string TYPE_DECISION = "TYPE_DECISION"; // Loại quyết định
        public const string NATION = "NATION"; //Dân tộc
        public const string NATIONALITY = "NATIONALITY"; // Quốc tịch
        public const string RELIGION = "RELIGION"; // Tôn giáo
        public const string OBJECT_ORG = "OBJECT_ORG"; // Tôn giáo
        public const string OBJECT_EMP = "OBJECT_EMP"; // Tôn giáo
        public const string INS_REGION = "INS_REGION"; // Vùng
        public const string INS_UNIT = "INS_UNIT"; // Đơn vị bảo hiểm
        
        public const string WORKING_BEFORE = "00042";// code qua  trinh cong tac truoc day
        public const string WORKING = "00043";// code qua  trinh cong tac
    }
    public class OtherConfig
    {

        //Đối tượng khen thưởng xử phạt
        public const int OBJECT_ORG = 1;
        public const int OBJECT_EMP = 2;

        //Nguôn chi
        public const int SOURCE_COMPANY_FUNDS = 1;//Quỹ công ty

        // Trạng thái phê duyệt 
        public const int STATUS_WAITING = 993;
        public const int STATUS_APPROVE = 994;          // Phê duyệt
        public const int STATUS_DECLINE = 995;          // Không phê duyệt

        // Khen thưởng
        public const int COMMEND_PERSIONAL = 0;
        public const int COMMEND_COLLECTIVE = 1;
        // Kỷ luật
        public const int DISCIPLINE_PERSIONAL = 0;
        public const int DISCIPLINE_COLLECTIVE = 1;
        // Trạng thái nhân viên
        public const int EMP_STATUS_WORKING = 1027;
        public const int EMP_STATUS_MATERNITY = 3;
        public const int EMP_STATUS_TERMINATE = 1028;
        // Sắp xếp
        public const int SORT_FML_PAYROLL = 1; // SẮP XẾP CÔNG THỨC LƯƠNG
        public const int SORT_FML_KPI = 2; // SẮP XẾP CÔNG THỨC KPI
        public const int SORT_FML_STRUCT = 3; // SẮP XẾP THÔNG TIN KẾT CẤU BẢNG LƯƠNG


        // Loại quyết định
        public const int POSTPONE_CONTRACT = 1172;   // Tạm hoãn hợp đồng
        public const int TYPE_TER_REASON = 1051;   // Nghỉ hưu

        //Set up thoi gian hien thi tb
        public const int NUM_OFF_NOTIFICATION = 30;

        
    }
    public class Message
    {
        public const string OBJECT_COMMEND_NOTE_EXIST = "OBJECT_COMMEND_NOTE_EXIST";
        public const string ORG_NOT_EXIST = "ORG_NOT_EXIST";
        public const string EMP_NOT_EXIST = "EMP_NOT_EXIST";
        public const string POSITION_NOT_EXIST = "POSITION_NOT_EXIST";
        public const string CODE_EXIST = "CODE_EXIST";
        public const string SIGNER_NOT_EXIST = "SIGNER_NOT_EXIST";
        public const string STATUS_NOT_EXIST = "STATUS_NOT_EXIST";
        public const string STATUS_BHXH_NOT_EXIST = "STATUS_BHXH_NOT_EXIST";
        public const string RECORD_EXIST_APPROVED = "RECORD_EXIST_APPROVED";
        public const string RECORD_IS_APPROVED = "RECORD_IS_APPROVED";
        public const string RECORD_NOT_FOUND = "RECORD_NOT_FOUND";
        public const string TIME_SHEET_DAILY_NOT_EXIST = "TIME_SHEET_DAILY_NOT_EXIST";
        public const string TIME_SHEET_LOCKED = "TIME_SHEET_LOCKED"; // bang cong da khoa
        public const string TIME_SHEET_UNLOCKED = "TIME_SHEET_UNLOCKED"; // bang cong  chua khoa
        public const string TYPE_NOT_EXIST = "TYPE_NOT_EXIST";
        public const string INVALID_FORMULA = "INVALID_FORMULA"; // sai cong thuc
        public const string RELATION_SHIP_NOT_EXIST = "RELATION_SHIP_NOT_EXIST"; // Không tồn tại quan hệ
        public const string EXPIRE_DATE = "EXPIRE_DATE"; // Hết hạn trả lời bình chọn câu hỏi
        public const string NOT_MULTIPLE = "NOT_MULTIPLE"; // Không được bình chọn nhiều
        public const string NOT_ADD_ANSWER = "NOT_ADD_ANSWER"; // Không được bình chọn nhiều

        public const string DATA_IS_USED = "DATA_IS_USED";
        public const string DATE_IS_EXISTS = "DATE_IS_EXISTS";
        public const string DATA_IS_EXISTS = "DATA_IS_EXISTS";
        public const string DAY_YEAR_NOT_ENOUGH = "DAY_YEAR_NOT_ENOUGH";
        public const string MASTER_DONOT_DECISION_SWAP = "MASTER_DONOT_DECISION_SWAP";
        public const string DO_NOT_INSERT_EMPLOYEE_POSITION = "DO_NOT_INSERT_EMPLOYEE_POSITION";
        public const string THE_POSITION_IS_SITTING = "THE_POSITION_IS_SITTING";
        public const string EMPLOYEE_ARE_WORKING = "EMPLOYEE_ARE_WORKING";
    }
    public class Procedures
    {
        public const String PKG_SYSTEM_FORM_ELEMENT = "PKG_SYSTEM.FORM_ELEMENT";//da thay the
        public const String PKG_SYSTEM_FORM_LIST ="PKG_SYSTEM.FORM_LIST"   ;//da thay the
        public const String PKG_IMPORT_ADVANCE_DATA_IMPOR ="PKG_IMPORT.ADVANCE_DATA_IMPOR"   ;
        public const String PKG_IMPORT_ADVANCE_IMPORT ="PKG_IMPORT.ADVANCE_IMPORT"   ;//da thay the
        public const String PKG_KPI_GET_EMP_KPI_IMP = "PKG_KPI.GET_EMP_KPI_IMP";//da thay    ;
        public const String PKG_KPI_CALC_KPI_SALARY ="PKG_KPI.CALC_KPI_SALARY"   ;//da thay the
        public const String PKG_KPI_IMPORT_KPI ="PKG_KPI.IMPORT_KPI"   ;// da thay the
        public const String PKG_PAYROLL_LOCK_KPI ="PKG_PAYROLL.LOCK_KPI"   ;//da thay the
        public const String PKG_IMPORT_SAL_IMPORT_LIST = "PKG_IMPORT.SAL_IMPORT_LIST"   ;//da thay the

        public const String PKG_COMMON_LIST_ORG = "PKG_COMMON.LIST_ORG"; //da thay the cho tat ca cac function
        public const String PKG_ENTITLEMENT_CREATED_BY_CONTRACT = "PKG_ENTITLEMENT.CREATED_BY_CONTRACT";//da thay the
        public const String PKG_IMPORT_CONTRACT_DATA_IMPORT = "PKG_IMPORT.CONTRACT_DATA_IMPORT";//da thay the
        public const String PKG_PROFILE_GET_INFO = "PKG_PROFILE.GET_INFO";//da thay the cho tat ca cac function
        public const String PKG_COMMON_LIST_RULE = "PKG_COMMON.LIST_RULE";//da thay the cho tat ca cac function
        public const String PKG_PROFILE_IMPORT_EMP = "PKG_PROFILE.IMPORT_EMP";
        public const String PKG_PROFILE_GET_DATA_IMPORT = "PKG_PROFILE.GET_DATA_IMPORT";//da thay the
        public const String PKG_PROFILE_PHONE_BOOK = "PKG_PROFILE.PHONE_BOOK";//da thay the
        public const String PKG_PROFILE_PHONE_BOOK_DTL = "PKG_PROFILE.PHONE_BOOK_DTL";//da thay the
        public const String PKG_REPORT_REPORT_INS = "PKG_REPORT.REPORT_INS";//da thay the
        public const String PKG_REPORT_REPORT_INS_PERIOD = "PKG_REPORT.REPORT_INS_PERIOD";//da thay the
        public const String PKG_REPORT_REPORT_HU001 = "PKG_REPORT.REPORT_HU001";//da thay the
        public const String PKG_REPORT_REPORT_HU009 = "PKG_REPORT.REPORT_HU009";//da thay the
        public const String PKG_IMPORT_DECISION_DATA_IMPORT = "PKG_IMPORT.DECISION_DATA_IMPORT";//da thay the

        public const String PKG_IMPORT_DECISION_IMPORT =   "PKG_IMPORT.DECISION_IMPORT";                          
        public const String PKG_IMPORT_INSURANCE_DATA_IMPORT =   "PKG_IMPORT.INSURANCE_DATA_IMPORT";  // da thay the                      
        public const String PKG_IMPORT_INSURANCE_IMPORT =   "PKG_IMPORT.INSURANCE_IMPORT";      // da thay the                       
        public const String PKG_PAYROLL_ADD_ALLOWANCE =   "PKG_PAYROLL.ADD_ALLOWANCE";     // da thay the                    
        public const String PKG_DEMO_CHECK_ALLOWANCE_USED =   "PKG_DEMO.CHECK_ALLOWANCE_USED";//da thay the
        public const String PKG_OMS_REPORT_GET_ORGANIZATION_CHART =   "PKG_OMS_REPORT.GET_ORGANIZATION_CHART";  //da thay the               
        public const String PKG_OMS_REPORT_IU_RPT_JOB_POS_HIS =   "PKG_OMS_REPORT.IU_RPT_JOB_POS_HIS";  //da thay the                        
        public const String PKG_OMS_REPORT_GET_JOB_CHILD_TREE =   "PKG_OMS_REPORT.GET_JOB_CHILD_TREE";  // da thay the                     
        public const String PKG_OMS_BUSINESS_JOB_AUTO_UPDATE_POSITION_TEMP =   "PKG_OMS_BUSINESS.JOB_AUTO_UPDATE_POSITION_TEMP";//da thay the
        public const String PKG_COMMON_LIST_USER_ORG = "PKG_COMMON.LIST_USER_ORG";//da thay the cho tat ca cac function
        public const String PKG_COMMON_LIST_ORG_PERMISSION = "PKG_COMMON.LIST_ORG_PERMISSION";//da thay the cho tat ca cac function
        public const String PKG_SYNC_SWIPE_SYNC_MACHINE = "PKG_SYNC.SWIPE_SYNC_MACHINE"; //lay du lieu mcc

        public const String PKG_PAYROLL_SETTING_ELEMENT_FORMULA_CHANGE = "PKG_PAYROLL_SETTING.ELEMENT_FORMULA_CHANGE";//da thay the
        public const String PKG_PAYROLL_SETTING_STRUCT_REMOVE_ELEMENT = "PKG_PAYROLL_SETTING.STRUCT_REMOVE_ELEMENT";
        public const String PKG_PROFILE_LIST_EMPLOYEE = "PKG_PROFILE.LIST_EMPLOYEE";
        public const String PKG_PAYROLL_ADD_SHIFT = "PKG_PAYROLL.ADD_SHIFT";
        public const String PKG_PAYROLL_LIST_PAYROLL_SUM = "PKG_PAYROLL.LIST_PAYROLL_SUM";
        public const String PKG_PAYROLL_M_PAYROLL_SUM = "PKG_PAYROLL.M_PAYROLL_SUM";
        public const String PKG_PAYROLL_CHECK_TIMESHEET_LOCK = "PKG_PAYROLL.CHECK_TIMESHEET_LOCK";
        public const String PKG_PAYROLL_LIST_IMPORT = "PKG_PAYROLL.LIST_IMPORT";
        public const String PKG_IMPORT_ADVANCE_DATA_IMPORT = "PKG_IMPORT.ADVANCE_DATA_IMPORT";
        public const String PKG_PAYROLL_SETTING_ELEMENT_FORMULA_KPI_CHANGE  = "PKG_PAYROLL_SETTING.ELEMENT_FORMULA_KPI_CHANGE";
        public const String PKG_COMMON_FORM_LIST = "PKG_COMMON.FORM_LIST";
        public const String PKG_COMMON_FORM_PRINT_SALARY = "PKG_COMMON.FORM_PRINT_SALARY";
        public const String PKG_COMMON_FORM_PRINT_ATTENDANCE = "PKG_COMMON.FORM_PRINT_ATTENDANCE";
        public const String PKG_COMMON_FORM_ELEMENT = "PKG_COMMON.FORM_ELEMENT";
        public const String PKG_PORTAL_VOTE_LIST = "PKG_PORTAL.VOTE_LIST";
        public const String PKG_NOTIFY_PORTAL_COUNT_REG = "PKG_NOTIFY.PORTAL_COUNT_REG";
        public const String PKG_NOTIFY_PORTAL_COUNT_HOME = "PKG_NOTIFY.PORTAL_COUNT_HOME";
        public const String PKG_PROFILE_GET_NOTIFY = "PKG_PROFILE.GET_NOTIFY";
        public const String PKG_COMMON_CLONE_TABLE_PAYROLL = "PKG_COMMON.CLONE_TABLE_PAYROLL";
        public const String PKG_DASHBOARD_STATISTIC_COMMON = "PKG_DASHBOARD_STATISTIC_COMMON";
        public const String PKG_PAYROLL_ADD_ELEMENT_SAL = "PKG_PAYROLL.ADD_ELEMENT_SAL";
        public const String PKG_REPORT_RPT_AT002 = "PKG_REPORT.RPT_AT002";
        public const String PKG_REPORT_RPT_PA002 = "PKG_REPORT.RPT_PA002";
        public const String PKG_REPORT_RPT_AT003 = "PKG_REPORT.RPT_AT003";
        public const String PKG_PROFILE_BUSSINESS_GET_WELFARE_AUTO = "PKG_PROFILE_BUSSINESS.GET_WELFARE_AUTO";
        public const String PKG_ATTENDANCE_CALCULATE_CAL_TIME_TIMESHEET_MONTHLY = "PKG_ATTENDANCE_CALCULATE.CAL_TIME_TIMESHEET_MONTHLY";
        public const String PKG_ATTENDANCE_CALCULATE_CAL_TIME_TIMESHEET_MACHINE = "PKG_ATTENDANCE_CALCULATE.CAL_TIME_TIMESHEET_MACHINE";
        public const String PKG_ATTENDANCE_BUSINESS_CALL_ENTITLEMENT = "PKG_ATTENDANCE_BUSINESS.CALL_ENTITLEMENT";
        public const String PKG_AT_PROCESS_PRI_PROCESS = "PKG_AT_PROCESS.PRI_PROCESS";
        public const String PKG_AT_PROCESS_PRI_PROCESS_APP = "PKG_AT_PROCESS_PRI_PROCESS_APP";
        public const String PKG_CHECK_ACTIVE_ORG = "PKG_CHECK_ACTIVE_ORG";
        


    }
}
