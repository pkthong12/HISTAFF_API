using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.VisualBasic;
using System.Net;

namespace API.All.SYSTEM.Common
{
        public static class CommonMessageCodes
        {
                public static readonly string ALERT_WRONG_ACCOUNT = "ALERT_WRONG_ACCOUNT";
                public static readonly string ALERT_CORRECT_ACCOUNT = "ALERT_CORRECT_ACCOUNT";
                public static readonly string ALERT_CORRECT_VERIFICATION_CODE = "ALERT_CORRECT_VERIFICATION_CODE";
                public static readonly string ALERT_WRONG_VERIFICATION_CODE = "ALERT_WRONG_VERIFICATION_CODE";
                public static readonly string THERE_IS_NO_COMPANY_EMAIL_IN_YOUR_PROFILE_YET = "THERE_IS_NO_COMPANY_EMAIL_IN_YOUR_PROFILE_YET";
                public static readonly string QUERY_RESULT_IS_NULL = "QUERY_RESULT_IS_NULL";
                public static readonly string YOU_DO_NOT_CHOOSE_ORGANIZATION_NEED_TRANSFER = "YOU_DO_NOT_CHOOSE_ORGANIZATION_NEED_TRANSFER";
                public static readonly string YOU_DO_NOT_CHOOSE_RECORD_NEED_TRANSFER = "YOU_DO_NOT_CHOOSE_RECORD_NEED_TRANSFER";
                public static readonly string DUPLICATE_ORGID = "DUPLICATE_ORGID";
                public static readonly string DUPLICATE_CHAIR = "DUPLICATE_CHAIR";
                public static readonly string TRANSFER_POSITION_SUCCESS = "TRANSFER_POSITION_SUCCESS";
                public static readonly string YOU_DO_NOT_CHOOSE_RECORD_NEED_CLONING = "YOU_DO_NOT_CHOOSE_RECORD_NEED_CLONING";
                public static readonly string YOU_DO_NOT_CHOOSE_ORGANIZAITION_NEED_CLONING = "YOU_DO_NOT_CHOOSE_ORGANIZAITION_NEED_CLONING";
                public static readonly string AMOUNT_CLONING_MUST_GREATER_THAN_1 = "AMOUNT_CLONING_MUST_GREATER_THAN_1";
                public static readonly string CAN_NOT_CLONING_HEAD_OF_DEPARTMENT = "CAN_NOT_CLONING_HEAD_OF_DEPARTMENT";
                public static readonly string CLONING_POSITION_SUCCESS = "CLONING_POSITION_SUCCESS";
                public static readonly string REVERT_SUCCESS = "REVERT_SUCCESS";
                public static readonly string NO_MANIPULATIONS_HAVE_BEEN_PERFORMED_YET = "NO_MANIPULATIONS_HAVE_BEEN_PERFORMED_YET";
                public static readonly string NO_SELECTED_ID_TO_DELETE = "NO_SELECTED_ID_TO_DELETE";
                public static readonly string NO_SELECTED_ID_TO_CREATE = "NO_SELECTED_ID_TO_CREATE";
                public static readonly string WORK_STATUS_ID_NOT_FOUND = "WORK_STATUS_ID_NOT_FOUND";
                public static readonly string NO_SELECTED_HU_COMPETENCY_PERIOD_ID_TO_CREATE = "NO_SELECTED_HU_COMPETENCY_PERIOD_ID_TO_CREATE";
                public static readonly string ONLY_PRINT_A_RECORD = "ONLY_PRINT_A_RECORD";
                public static readonly string NO_PROBATION_CONTRACT_AND_LABOR_CONTRACT = "NO_PROBATION_CONTRACT_AND_LABOR_CONTRACT";
                public static readonly string NO_EXISIT_DECISION_PRINT_TEMPLATE = "NO_EXISIT_DECISION_PRINT_TEMPLATE";
                public static readonly string ID_OF_DECISION_DIFFERENT_ID_OF_SYS_OTHER_LIST = "ID_OF_DECISION_DIFFERENT_ID_OF_SYS_OTHER_LIST";
                public static readonly string NOT_FOUND_RECORD_OF_WORKING_PROCESS = "NOT_FOUND_RECORD_OF_WORKING_PROCESS";
                public static readonly string NOT_FOUND_SUITABLE_APPENDIX_CONTRACT = "NOT_FOUND_SUITABLE_APPENDIX_CONTRACT";
                public static readonly string THERE_IS_NO_AVAILABILITY_MINIMUM_WAGE = "THERE_IS_NO_AVAILABILITY_MINIMUM_WAGE";
                public static readonly string ITEMID_HAS_EXISTS = "ITEMID_HAS_EXISTS";
                public static readonly string TAX_CODE_HAVE_EXISTS = "TAX_CODE_HAVE_EXISTS";
                public static readonly string NO_REFRESHTOKEN_INFORMATION_FOUND_EITHER_IN_COOKIE_AND_PAYLOAD = "NO_REFRESHTOKEN_INFORMATION_FOUND_EITHER_IN_COOKIE_AND_PAYLOAD";
                public static readonly string PAYLOAD_BASED_REFRESHTOKEN_PROVIDED_WAS_EMPTY = "PAYLOAD_BASED_REFRESHTOKEN_PROVIDED_WAS_EMPTY";
                public static readonly string NO_USER_MATCHS_PROVIDED_REFRESHTOKEN = "NO_USER_MATCHS_PROVIDED_REFRESHTOKEN";
                public static readonly string REVOKED_REFRESHTOKEN = "REVOKED_REFRESHTOKEN";
                public static readonly string INACTIVATED_REFRESHTOKEN = "INACTIVATED_REFRESHTOKEN";
                public static readonly string EXIST_PAYROLL_FUND = "EXIST_PAYROLL_FUND";
                public static readonly string DUPLICATE_TAX_CODE = "DUPLICATE_TAX_CODE";
                public static readonly string DUPLICATE_EMPLOYEE_CODE = "DUPLICATE_EMPLOYEE_CODE";
                public static readonly string REQURIED_FULL_NAME = "REQURIED_FULL_NAME";
                public static readonly string REQURIED_ORG_ID = "REQURIED_ORG_ID";
                public static readonly string REQURIED_POSITION_ID = "REQURIED_POSITION_ID";
                public static readonly string REQURIED_EMPLOYEE_OBJECT_ID = "REQURIED_EMPLOYEE_OBJECT_ID";
                public static readonly string REQURIED_GENDER_ID = "REQURIED_GENDER_ID";
                public static readonly string REQURIED_BIRTH_DATE = "REQURIED_BIRTH_DATE";
                public static readonly string REQURIED_PROVINCE_ID = "REQURIED_PROVINCE_ID";
                public static readonly string REQURIED_DISTRICT_ID = "REQURIED_DISTRICT_ID";
                public static readonly string REQURIED_WARD_ID = "REQURIED_WARD_ID";
                public static readonly string REQURIED_ADDRESS = "REQURIED_ADDRESS";
                public static readonly string IMPORT_SUCCESS = "IMPORT_SUCCESS";
                public static readonly string CAN_NOT_DELETE_DATA_FROM_HU_WORKING = "CAN_NOT_DELETE_DATA_FROM_HU_WORKING";
                public static readonly string DUPLICATE_ID_NO = "DUPLICATE_ID_NO";
                public static readonly string MOBILE_IS_NOT_ALLOWED = "MOBILE_IS_NOT_ALLOWED";
        }
}