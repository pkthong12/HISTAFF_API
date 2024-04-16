using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AD_PROGRAMS",
                columns: table => new
                {
                    PROGRAM_ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROGRAM_TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STORE_EXECUTE_IN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STORE_EXECUTE_OUT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TEMPLATE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TEMPLATE_TYPE_IN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TEMPLATE_TYPE_OUT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TEMPLATE_URL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AD_PROGRAMS", x => x.PROGRAM_ID);
                });

            migrationBuilder.CreateTable(
                name: "AD_REQUESTS",
                columns: table => new
                {
                    REQUEST_ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PROGRAM_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PHASE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    START_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    END_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ACTUAL_START_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ACTUAL_COMPLETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    OUTPUTFILE_SIZE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    NLS_LANGUAGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NLS_TERRITORY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRINTER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    size = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    ORIENTATION = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    PERMISSION = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STORE_EXECUTE_IN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STORE_EXECUTE_OUT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRIORITY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    LOG_FILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TEMPLATE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TEMPLATE_TYPE_IN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TEMPLATE_TYPE_OUT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TEMPLATE_URL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FILE_OUT_URL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FILE_OUT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CSV_FILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TRY_COUNT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AD_ROOM",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AD_ROOM", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    UserName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AT_CONFIG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ADVANCE_NUMBER = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    DATE_CLEAR = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_CONFIG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_DAYOFFYEAR",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    YEAR_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DAY_OFF = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_1 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_2 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_3 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_4 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_5 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_6 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_7 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_8 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_9 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_10 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_11 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_12 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_DAYOFFYEAR", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_DAYOFFYEAR_CONFIG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ADVANCE_NUMBER = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    IS_INTERN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_ACCUMULATION = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MONTH_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_DAYOFFYEAR_CONFIG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_DECLARE_SENIORITY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    YEAR_DECLARE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MONTH_ADJUST = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MONTH_ADJUST_NUMBER = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REASON_ADJUST = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MONTH_DAY_OFF = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NUMBER_DAY_OFF = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REASON_ADJUST_DAY_OFF = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TOTAL = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_DECLARE_SENIORITY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_ENTITLEMENT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_TIME_HAVE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_HAVE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE1 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE2 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE3 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE4 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE5 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE6 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE7 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE8 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE9 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE10 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE11 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_HAVE12 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED1 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED2 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED3 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED4 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED5 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED6 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED7 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED8 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED9 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED10 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED11 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CUR_USED12 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    MONTH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PREVTOTAL_HAVE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED1 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED2 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED3 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED4 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED5 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED6 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED7 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED8 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED9 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED10 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED11 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PREV_USED12 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    EXPIREDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOIN_DATE_STATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SENIORITY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    SENIORITYHAVE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    SENIORITY_MONTH = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    QP_YEAR = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    QP_MONTH_SUM = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_HAVE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    QP_YEARX_USED = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    QP_YEARX_HAVE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    QP_STANDARD = table.Column<double>(type: "BINARY_DOUBLE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_ENTITLEMENT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_ENTITLEMENT_EDIT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MONTH = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NUMBER_CHANGE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE_REF = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_ENTITLEMENT_EDIT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_HOLIDAY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    START_DAYOFF = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    END_DAYOFF = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    WORKINGDAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_HOLIDAY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_NOTIFICATION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ACTION = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTIFI_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMP_CREATE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FMC_TOKEN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_READ = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    TENANT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REF_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TITLE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMP_NOTIFY_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_NOTIFY = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODEL_CHANGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_NOTIFICATION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_ORG_PERIOD",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    STATUSCOLEX = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    STATUSPAROX = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    STATUSPAROX_SUB = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    STATUSPAROX_BACKDATE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    STATUSPAROX_TAX_MONTH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    STATUSPAROX_TAX_YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    UPTO_PORTAL = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    STATUSEXP = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    STATUSPAYBACK = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_ORG_PERIOD", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_OTHER_LIST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_ENTIRE_YEAR = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MAX_WORKING_MONTH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MAX_WORKING_YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    OVERTIME_DAY_WEEKDAY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OVERTIME_DAY_HOLIDAY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OVERTIME_DAY_OFF = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OVERTIME_NIGHT_WEEKDAY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OVERTIME_NIGHT_HOLIDAY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OVERTIME_NIGHT_OFF = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PERSONAL_DEDUCTION_AMOUNT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SELF_DEDUCTION_AMOUNT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    BASE_SALARY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    WORKDAY_UNIT_PRICE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_OTHER_LIST", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_OVERTIME",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TIME_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TIME_END = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    START_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    END_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_OVERTIME", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_OVERTIME_CONFIG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    HOUR_MIN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    HOUR_MAX = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    FACTOR_NT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    FACTOR_NN = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    FACTOR_NL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    FACTOR_DNT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    FACTOR_DNN = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    FACTOR_DNL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_OVERTIME_CONFIG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_PERIOD_STANDARD",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OBJECT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PERIOD_STANDARD = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PERIOD_STANDARD_NIGHT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_PERIOD_STANDARD", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_REGISTER_LEAVE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DATE_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DATE_END = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TYPE_OFF = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_EACH_DAY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_REGISTER_LEAVE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_REGISTER_LEAVE_DETAIL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    LEAVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    REGISTER_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    MANUAL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TYPE_OFF = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NUMBER_DAY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_REGISTER_LEAVE_DETAIL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_SALARY_PERIOD",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    START_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    END_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    STANDARD_WORKING = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MONTH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_SALARY_PERIOD", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_SETUP_TIME_EMP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NUMBER_SWIPECARD = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ACTFLG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_SETUP_TIME_EMP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_SHIFT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    HOURS_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    HOURS_STOP = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    BREAKS_FROM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BREAKS_TO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TIME_LATE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TIME_EARLY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TIME_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    IS_BREAK = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_BOQUACC = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    TIME_START = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TIME_STOP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MIN_HOURS_WORK = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_NIGHT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SUNDAY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    SUNDAY = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SATURDAY = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_SHIFT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_SIGN_DEFAULT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECT_DATE_FROM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EFFECT_DATE_TO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SIGN_DEFAULT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ACTFLG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_SIGN_DEFAULT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_SWIPE_DATA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    VALTIME = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TIME_ONLY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TERMINAL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_DAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EVT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_GPS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ADDRESS_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    LATATUIDE_LONGTATUIDE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SHIFT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_SWIPE_DATA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_SWIPE_DATA_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TERMINAL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_DAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VALTIME = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TIME_ONLY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EVT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_GPS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ADDRESS_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EMP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LATATUIDE_LONGTATUIDE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SHIFT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_SWIPE_DATA_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "AT_SWIPE_DATA_SYNC",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TIME_POINT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    REF_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TIME_POINT_STR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_SWIPE_DATA_SYNC", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_SWIPE_DATA_WRONG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TENANT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EMP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TIME_POINT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    LATITUDE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LONGITUDE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODEL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IMAGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MAC = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OPERATING_SYSTEM = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OPERATING_VERSION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WIFI_IP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BSS_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_SWIPE_DATA_WRONG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_SYMBOL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_OFF = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_HOLIDAY_CAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_INS_ARISING = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_REGISTER = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_HAVE_SAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    WORKING_HOUR = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_SYMBOL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_TERMINAL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TERMINAL_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TERMINAL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADDRESS_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TERMINAL_PORT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TERMINAL_IP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PASS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_TERMINAL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_TIME_EXPLANATION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EXPLANATION_DAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SHIFT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SWIPE_IN = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SWIPE_OUT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ACTUAL_WORKING_HOURS = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EXPLANATION_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_TIME_EXPLANATION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_TIME_TIMESHEET_DAILY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKINGDAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SHIFT_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEAVE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LATE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    COMEBACKOUT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKDAY_OT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKDAY_NIGHT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TYPE_DAY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VALIN1 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VALIN2 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VALIN3 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VALIN4 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DECISION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PA_OBJECT_SALARY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SHIFT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MANUAL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LEAVE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKINGHOUR = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKINGHOUR_SHIFT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    NUMBER_SWIPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MINUTE_DM_VIPHAM = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    IS_GIAITRINH = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MANUAL_ID_TK = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    LEAVE_ID_TK = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    LEAVE_CODE_TK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DIMUON_VESOM_THUCTE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_WEEKDAY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_WEEKNIGHT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_SUNDAY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_SUNDAYNIGHT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_HOLIDAY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_HOLIDAY_NIGHT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_TOTAL_CONVERT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_WEEKDAY_TC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_WEEKNIGHT_TC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_SUNDAY_TC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_SUNDAYNIGHT_TC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_HOLIDAY_TC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_HOLIDAY_NIGHT_TC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_WEEKDAY_SC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_WEEKNIGHT_SC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_SUNDAY_SC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_SUNDAYNIGHT_SC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_HOLIDAY_SC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_HOLIDAY_NIGHT_SC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    IS_NB_TC = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_NB_SC = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    OT_TOTAL_CONVERT_PAY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_TOTAL_CONVERT_NB = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    IS_EDIT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MINUTE_OUT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    HR_COMMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DETAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_REGISTER_LATE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_REGISTER_COMEBACKOUT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    TOTAL_MINUTE_OUT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    IS_NO_LATE_COMEBACKOUT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_CONFIRM = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MINUTE_DM_DK = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    MINUTE_VS_DK = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    SHIFT_ID_OT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_EDIT_INOUT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_OUT_REGISTER = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_AN_CA = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_HOLIDAY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_HOLIDAY_NEXT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_HOLIDAY_LT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_OFF_PRE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_OFF_NEXT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    GIO_VE_MUON = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_WEEKDAY_TT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_WEEKNIGHT_TT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_SUNDAY_TT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_SUNDAYNIGHT_TT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_HOLIDAY_TT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_HOLIDAY_NIGHT_TT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    VALIN1_TT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VALIN4_TT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SHIFT_MANUAL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    VAL_IN = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VAL_OUT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_OFF_DN1 = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_CASE_OT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE_COLOR = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_TIME_TIMESHEET_DAILY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_TIME_TYPE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    MORNING_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    AFTERNOON_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_OFF = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_TIME_TYPE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_TIMESHEET_FORMULA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FORMULA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FORMULA_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_TIMESHEET_FORMULA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_TIMESHEET_MONTHLY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_E = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DECISION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PA_EMPLOYEE_OBJECT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    STAFF_RANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    END_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TOTAL_WORKING_XJ = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_TS_V = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_WORKING = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_ADD = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    DECISION_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DECISION_END = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORKING_TV = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_A = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_NH = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_R = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_S = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_NB = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_J = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_TS = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_D = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_V = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_KT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_L = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_DS = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_H = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_NB = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_TOTAL_CONVERT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_OT_WEEKDAY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_OT_SUNDAY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_OT_SUNDAYNIGTH = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_OT_HOLIDAY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_OT_HOLIDAY_NIGTH = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_OT_STORE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_X = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_F = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_Q = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_NV = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_P = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_O = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_DA = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_VS = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_RO = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_CT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_KO = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_NH1 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_TOTAL_CONVERT_PAY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_TOTAL_CONVERT_NB = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_HL = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_KL = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_BHXH = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_OFF_KL = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_LATE_COMEBACKOUT_DK = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_LATE_COMEBACKOUT_KDK = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CHUYEN_CAN = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_150_NQD = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_210_NQD = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_200_NQD = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_270_NQD = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_300_NQD = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT_390_NQD = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_PAY_OT_NQD = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_LC = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_WORKING_HL = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_OT_WEEKNIGHT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_SHIFT_S3 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT150 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT210 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT200 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT270 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT300 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OT390 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_STANDARD = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    P_MAX_PAYOT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_STANDARD_GD = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    IS_SANCONG = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_LCB = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_CTN = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_LATE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_COMEBACKOUT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_DM_PHAT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TOTAL_VS_PHAT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WORKING_L_OFF = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    IS_TRAILWORK = table.Column<double>(type: "BINARY_DOUBLE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_TIMESHEET_MONTHLY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_WORKSIGN",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKINGDAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SHIFT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_WORKSIGN", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AT_WORKSIGN_TMP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REF_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    DAY_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_4 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_5 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_6 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_7 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_8 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_9 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_10 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_11 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_12 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_13 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_14 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_15 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_16 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_17 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_18 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_19 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_20 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_21 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_22 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_23 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_24 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_25 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_26 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_27 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_28 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_29 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_30 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY_31 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_WORKSIGN_TMP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CSS_THEME",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSS_THEME", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CSS_VAR",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSS_VAR", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DEMO_ATTACHMENT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    FIRST_ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SECOND_ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEMO_ATTACHMENT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DEMO_HANGFIRE_RECORD",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TEXT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DEMO_HANGFIRE_RECORD", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HRM_INFOTYPE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME_EN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME_VN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_INFOTYPE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HRM_OBJECT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_OBJECT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_ALLOWANCE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    COL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_INSURANCE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_FULLDAY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SALARY_RANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_COEFFICIENT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SAL = table.Column<bool>(type: "NUMBER(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_ALLOWANCE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_ALLOWANCE_EMP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ALLOWANCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MONNEY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    COEFFICIENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    DATE_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DATE_END = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_ALLOWANCE_EMP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_ALLOWANCE_EMP_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ALLOWANCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MONNEY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    COEFFICIENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    DATE_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DATE_END = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_ALLOWANCE_EMP_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_ANSWER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ANSWER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    QUESTION_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_ANSWER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_BANK",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SHORT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDER = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_BANK", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_BANK_BRANCH",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    BANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_BANK_BRANCH", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_CERTIFICATE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_PRIME = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    TYPE_CERTIFICATE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EFFECT_FROM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EFFECT_TO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TRAIN_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TRAIN_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    LEVEL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MAJOR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEVEL_TRAIN = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTENT_TRAIN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MARK = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    TYPE_TRAIN = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CODE_CETIFICATE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CLASSIFICATION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REMARK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_RECORD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_CERTIFICATE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_CERTIFICATE_EDIT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_PRIME = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    TYPE_CERTIFICATE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EFFECT_FROM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EFFECT_TO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TRAIN_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TRAIN_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    LEVEL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MAJOR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEVEL_TRAIN = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTENT_TRAIN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MARK = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    TYPE_TRAIN = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CODE_CETIFICATE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CLASSIFICATION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REMARK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_SEND_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_APPROVE_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MODEL_CHANGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ID_HU_CERTIFICATE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    STATUS_RECORD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ID_SYS_OTHER_LIST_APPROVE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_SAVE_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_CERTIFICATE_EDIT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_CERTIFICATE_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_PRIME = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    TYPE_CERTIFICATE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EFFECT_FROM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EFFECT_TO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TRAIN_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TRAIN_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    LEVEL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MAJOR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEVEL_TRAIN = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTENT_TRAIN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MARK = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    TYPE_TRAIN = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CODE_CETIFICATE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CLASSIFICATION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REMARK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_RECORD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_CERTIFICATE_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_CLASSIFICATION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CLASSIFICATION_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CLASSIFICATION_LEVEL = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POINT_FROM = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POINT_TO = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_CLASSIFICATION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_COMMEND",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SIGN_PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COMMEND_OBJ_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SOURCE_COST_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COMMEND_TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MONEY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    IS_TAX = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CONTENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NUM_REWARD_PAY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POSITION_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SALARY_INCREASE_TIME = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ORG_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    AWARD_TITLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_PAYMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PAYMENT_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_PAYMENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_PAYMENT_PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_PAYMENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_PAYMENT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FUND_SOURCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REWARD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REWARD_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MONTH_TAX = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_PAYMENT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PAYMENT_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PAYMENT_CONTENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PAYMENT_ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LIST_REWARD_LEVEL_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_COMMEND", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_COMMEND_EMP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TENANT_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    COMMEND_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    POS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_COMMEND_EMP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_COMMEND_EMPLOYEE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COMMEND_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_COMMEND_EMPLOYEE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_COMMEND_EMPLOYEE_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COMMEND_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_COMMEND_EMPLOYEE_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_COMMEND_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SIGN_PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COMMEND_OBJ_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SOURCE_COST_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COMMEND_TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MONEY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    IS_TAX = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CONTENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NUM_REWARD_PAY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POSITION_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SALARY_INCREASE_TIME = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ORG_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    AWARD_TITLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_PAYMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PAYMENT_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_PAYMENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_PAYMENT_PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_PAYMENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_PAYMENT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FUND_SOURCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REWARD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REWARD_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MONTH_TAX = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_PAYMENT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PAYMENT_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PAYMENT_CONTENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PAYMENT_ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_COMMEND_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_COMPANY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME_VN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME_EN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    GPKD_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REGION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PHONE_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INS_UNIT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROVINCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DISTRICT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WARD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FILE_LOGO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_ACCOUNT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_BRANCH_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FILE_HEADER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PIT_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PIT_CODE_CHANGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PIT_CODE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    FILE_FOOTER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REPRESENTATIVE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PIT_CODE_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GPKD_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GPKD_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WEBSITE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAX = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    BANK_BRANCH = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDER = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SHORT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_COMPANY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_CONCURRENTLY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SIGNING_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    DECISION_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNING_EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNING_POSITION_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_FROM_WORKING = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    POSITION_POLITICAL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_CONCURRENTLY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_CONTRACT_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTRACT_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTRACT_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    START_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SAL_BASIC = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_PERCENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPLOAD_FILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_RECEIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_CONTRACT_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_DISCIPLINE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DECISION_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DECISION_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ISSUED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VIOLATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BASED_ON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DOCUMENT_SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DISCIPLINE_OBJ = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DISCIPLINE_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EXTEND_SAL_TIME = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_DISCIPLINE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_DISTRICT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROVINCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_DISTRICT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_DYNAMIC_REPORT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    JSON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EXPRESSION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REPORT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FORM = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VIEW_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SELECTED_COLUMNS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PREFIX_TRANS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_DYNAMIC_REPORT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_EMPLOYEE_CV",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    AVATAR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FIRST_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FULL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    OTHER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GENDER_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ID_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ID_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ID_PLACE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    RELIGION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NATIVE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NATIONALITY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CARRER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSURENCE_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEALTH_CARE_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INS_CARD_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROVINCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DISTRICT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WARD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CUR_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_PROVINCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CUR_DISTRICT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CUR_WARD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TAX_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MOBILE_PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MOBILE_PHONE_LAND = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_HOST = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    WORK_EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MARITAL_STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PASS_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRISON_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAMILY_DETAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PASS_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PASS_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PASS_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VISA_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_UNIONIST = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    UNIONIST_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UNIONIST_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    YOUTH_SAVE_NATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UNIONIST_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_SAVE_NATION_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_SAVE_NATION_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_JOIN_YOUTH_GROUP = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_MEMBER = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MEMBER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MEMBER_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LIVING_CELL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CARD_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POLITICAL_THEORY_LEVEL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESUME_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VATERANS_MEMBER_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VATERANS_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VATERANS_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_GROUP_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_GROUP_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_GROUP_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    HIGHEST_MILITARY_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CURRENT_PARTY_COMMITTEE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PARTYTIME_PARTY_COMMITTEE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ENLISTMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DISCHARGE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MEMBER_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MEMBER_OFFICAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PERMIT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PERMIT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_SCOPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTACT_PER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTACT_PER_PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HOUSEHOLD_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HOUSEHOLD_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_BRANCH = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_BRANCH_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_NO_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SCHOOL_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SCHOOL_ID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    QUALIFICATION_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    QUALIFICATIONID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    QUALIFICATIONID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    QUALIFICATIONID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EDUCATION_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TRAINING_FORM_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TRAINING_FORM_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TRAINING_FORM_ID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LEARNING_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE_MARK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IMAGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEIGHT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WEIGHT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DOMICILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_REGIS_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BLOOD_PRESSURE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEALTH_TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEFT_EYE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RIGHT_EYE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_CODE_ADDRESS = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    HEART = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEALTH_NOTES = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BLOOD_GROUP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAMILY_MEMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAMILY_POLICY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VETERANS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POLITICAL_THEORY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CARRER_BEFORE_RECUITMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TITLE_CONFERRED = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_OF_WORK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COMPUTER_SKILL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LICENSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_ID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_LEVEL_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_LEVEL_ID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_OBJECT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PRESENTER = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PRESENTER_PHONE_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRESENTER_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_CODE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXAMINATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MAIN_INCOME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OTHER_SOURCES = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAND_GRANTED = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_GRANTED_HOUSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TOTAL_AREA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SELF_PURCHASE_LAND = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SELF_BUILD_HOUSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TOTAL_APP_AREA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAND_FOR_PRODUCTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADDITIONAL_INFOMATION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GOVERMENT_MANAGEMENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YELLOW_FLAG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RELATIONS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INS_WHEREHEALTH_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COMPUTER_SKILL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LICENSE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EMPLOYEE_CV", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_EMPLOYEE_CV_EDIT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    AVATAR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FIRST_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FULL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OTHER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GENDER_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ID_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ID_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ID_PLACE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    RELIGION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NATIVE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NATIONALITY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CARRER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSURENCE_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEALTH_CARE_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INS_CARD_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROVINCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DISTRICT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WARD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CUR_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_PROVINCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CUR_DISTRICT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CUR_WARD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TAX_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MOBILE_PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MOBILE_PHONE_LAND = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_HOST = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    WORK_EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MARITAL_STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PASS_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRISON_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAMILY_DETAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PASS_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PASS_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PASS_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VISA_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_UNIONIST = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    UNIONIST_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UNIONIST_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    YOUTH_SAVE_NATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UNIONIST_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_SAVE_NATION_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_SAVE_NATION_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_JOIN_YOUTH_GROUP = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_MEMBER = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MEMBER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MEMBER_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LIVING_CELL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CARD_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POLITICAL_THEORY_LEVEL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESUME_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VATERANS_MEMBER_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VATERANS_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VATERANS_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_GROUP_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_GROUP_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_GROUP_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    HIGHEST_MILITARY_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CURRENT_PARTY_COMMITTEE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PARTYTIME_PARTY_COMMITTEE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ENLISTMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DISCHARGE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MEMBER_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MEMBER_OFFICAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PERMIT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PERMIT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_SCOPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTACT_PER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTACT_PER_PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HOUSEHOLD_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HOUSEHOLD_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_BRANCH_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_BRANCH_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_BRANCH = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_NO_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SCHOOL_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SCHOOL_ID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    QUALIFICATION_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    QUALIFICATIONID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    QUALIFICATIONID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    QUALIFICATIONID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EDUCATION_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TRAINING_FORM_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TRAINING_FORM_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TRAINING_FORM_ID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LEARNING_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE_MARK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IMAGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEIGHT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WEIGHT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DOMICILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_REGIS_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BLOOD_PRESSURE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEALTH_TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEFT_EYE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RIGHT_EYE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_CODE_ADDRESS = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    HEART = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEALTH_NOTES = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BLOOD_GROUP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAMILY_MEMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAMILY_POLICY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VETERANS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POLITICAL_THEORY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CARRER_BEFORE_RECUITMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TITLE_CONFERRED = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_OF_WORK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COMPUTER_SKILL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LICENSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_ID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_LEVEL_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_LEVEL_ID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PRESENTER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRESENTER_PHONE_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRESENTER_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_CODE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXAMINATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MAIN_INCOME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OTHER_SOURCES = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAND_GRANTED = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_GRANTED_HOUSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TOTAL_AREA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SELF_PURCHASE_LAND = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SELF_BUILD_HOUSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TOTAL_APP_AREA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAND_FOR_PRODUCTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADDITIONAL_INFOMATION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GOVERMENT_MANAGEMENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YELLOW_FLAG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RELATIONS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_SEND_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_APPROVED_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_APPROVED_CV = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_APPROVED_CONTACT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_APPROVED_ADDITIONAL_INFO = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_APPROVED_EDUCATION = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_APPROVED_BANK_INFO = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MODEL_CHANGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HU_EMPLOYEE_CV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_SEND_PORTAL_CV = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SEND_PORTAL_ADDITIONAL_INFO = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SEND_PORTAL_BANK_INFO = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SEND_PORTAL_EDUCATION = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SEND_PORTAL_CONTACT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    STATUS_APPROVED_CV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    STATUS_APPROVED_EDUCATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    STATUS_APPROVED_BANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    STATUS_ADDINATIONAL_INFO_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    STATUS_APPROVED_CONTACT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_APPROVED_INSUARENCE_INFO = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    STATUS_APPOVED_INSUARENCE_INFO_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    INS_WHEREHEALTH_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_SAVE_CV = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SAVE_CONTACT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SAVE_ADDITIONAL_INFO = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SAVE_BANK_INFO = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SAVE_EDUCATION = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SAVE_INSURENCE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SAVE_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    REASON_DISCARD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IDENTITY_ADDRESS = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ID_EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COMPUTER_SKILL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LICENSE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EMPLOYEE_CV_EDIT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_EMPLOYEE_CV_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ID_NO = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    AVATAR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FIRST_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FULL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OTHER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IMAGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GENDER_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ID_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ID_PLACE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NATIONALITY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROVINCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DISTRICT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WARD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CUR_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_PROVINCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CUR_DISTRICT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CUR_WARD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TAX_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MOBILE_PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MARITAL_STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PASS_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PASS_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PASS_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PASS_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VISA_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VISA_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTACT_PER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTACT_PER_PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_BRANCH = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    QUALIFICATION_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    QUALIFICATIONID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TRAINING_FORM_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LEARNING_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE_MARK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NATIVE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    RELIGION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORK_PERMIT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PERMIT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_SCOPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEIGHT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WEIGHT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DOMICILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_CODE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BIRTH_REGIS_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BLOOD_GROUP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BLOOD_PRESSURE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEALTH_TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEFT_EYE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RIGHT_EYE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEART = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EXAMINATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    HEALTH_NOTES = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CARRER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSURENCE_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEALTH_CARE_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INS_CARD_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAMILY_MEMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAMILY_POLICY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VETERANS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POLITICAL_THEORY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CARRER_BEFORE_RECUITMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TITLE_CONFERRED = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_OF_WORK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRISON_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAMILY_DETAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_UNIONIST = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    UNIONIST_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UNIONIST_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UNIONIST_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_JOIN_YOUTH_GROUP = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_MEMBER = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MEMBER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MEMBER_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MEMBER_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LIVING_CELL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CARD_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POLITICAL_THEORY_LEVEL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESUME_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VATERANS_MEMBER_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VATERANS_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VATERANS_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ENLISTMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DISCHARGE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    HIGHEST_MILITARY_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CURRENT_PARTY_COMMITTEE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PARTYTIME_PARTY_COMMITTEE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EDUCATION_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    QUALIFICATIONID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    QUALIFICATIONID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SCHOOL_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SCHOOL_ID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TRAINING_FORM_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TRAINING_FORM_ID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LICENSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_ID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_LEVEL_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LANGUAGE_LEVEL_ID_3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COMPUTER_SKILL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRESENTER = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PRESENTER_PHONE_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRESENTER_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_CODE_ADDRESS = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MEMBER_OFFICAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SCHOOL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    HOUSEHOLD_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_HOST = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    HOUSEHOLD_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MOBILE_PHONE_LAND = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MAIN_INCOME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OTHER_SOURCES = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAND_GRANTED = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_GRANTED_HOUSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TOTAL_AREA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SELF_PURCHASE_LAND = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SELF_BUILD_HOUSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TOTAL_APP_AREA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAND_FOR_PRODUCTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADDITIONAL_INFOMATION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_SAVE_NATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    YOUTH_SAVE_NATION_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_SAVE_NATION_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GOVERMENT_MANAGEMENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YELLOW_FLAG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RELATIONS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_GROUP_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_GROUP_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    YOUTH_GROUP_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_ID_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_BRANCH_2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_NO_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_ACCOUNT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_OBJECT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    INS_WHEREHEALTH_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COMPUTER_SKILL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LICENSE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EMPLOYEE_CV_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.ID_NO });
                });

            migrationBuilder.CreateTable(
                name: "HU_EMPLOYEE_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTRACT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTRACT_EXPIRED = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CONTRACT_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    RESIDENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    LAST_WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DIRECT_MANAGER_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TER_EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SALARY_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SENIORITY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    WORK_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SAL_TOTAL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_BASIC = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_RATE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    DAY_OF = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    STAFF_RANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORK_STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_OBJECT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LABOR_OBJECT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    STATUS_DETAIL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    INSURENCE_AREA_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    JOIN_DATE_STATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TER_LAST_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    INSURENCE_AREA = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_NOT_CONTRACT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EMPLOYEE_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_EMPLOYEE_PAPERS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PAPER_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    EMP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DATE_INPUT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    URL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EMPLOYEE_PAPERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_EMPLOYEE_TMP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    REF_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FIRST_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    LAST_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    FULLNAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POS_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    RESIDENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESIDENT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    GENDER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GENDER_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    BIRTH_DATE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_DATE_INPUT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ID_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ID_DATE_INPUT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ID_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RELIGION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RELIGION_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NATIVE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NATIVE_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NATIONALITY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NATIONALITY_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROVINCE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROVINCE_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DISTRICT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DISTRICT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    WARD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WARD_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CUR_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_PROVINCE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_PROVINCE_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CUR_DISTRICT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_DISTRICT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CUR_WARD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_WARD_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTACT_PER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTACT_PER_PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MOBILE_PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MARITAL_STATUS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MARITAL_STATUS_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PASS_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PASS_DATE_INPUT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PASS_EXPIRE_INPUT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PASS_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VISA_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VISA_DATE_INPUT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_EXPIRE_INPUT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PERMIT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PERMIT_DATE_INPUT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT_EXPIRE_INPUT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_BRANCH = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SCHOOL_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMP_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    QUALIFICATION_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TRAINING_FORM = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TRAINING_FORM_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    LEARNING_LEVEL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEARNING_LEVEL_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    LANGUAGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EMPLOYEE_TMP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_EVALUATE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EVALUATE_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CLASSIFICATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POINT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POSITION_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POSITION_CONCURRENT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_CONCURRENT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POSITION_CONCURRENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_CONCURRENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_CONCURRENT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_CONCURRENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EVALUATE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_EVALUATE_CONCURRENT_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EVALUATE_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CLASSIFICATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POINT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POSITION_CONCURRENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_CONCURRENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_CONCURRENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EVALUATE_CONCURRENT_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_EVALUATE_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EVALUATE_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CLASSIFICATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POINT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POSITION_CONCURRENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_CONCURRENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_CONCURRENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EVALUATE_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION });
                });

            migrationBuilder.CreateTable(
                name: "HU_EVALUATION_COM",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR_EVALUATION = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CLASSIFICATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POINT_EVALUATION = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EVALUATION_COM", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_EVALUATION_COM_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR_EVALUATION = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CLASSIFICATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POINT_EVALUATION = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EVALUATION_COM_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_FAMILY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    RELATIONSHIP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FULLNAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GENDER = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PIT_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SAME_COMPANY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_DEAD = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_DEDUCT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    DEDUCT_FROM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DEDUCT_TO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REGIST_DEDUCT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_HOUSEHOLD = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    ID_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CAREER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NATIONALITY = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_CER_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_CER_PROVINCE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_CER_DISTRICT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_CER_WARD = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    UPLOAD_FILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_FAMILY", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_FAMILY_EDIT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    RELATIONSHIP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FULLNAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GENDER = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PIT_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SAME_COMPANY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_DEAD = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_DEDUCT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    DEDUCT_FROM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DEDUCT_TO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REGIST_DEDUCT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_HOUSEHOLD = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    ID_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CAREER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NATIONALITY = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_CER_PROVINCE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_CER_DISTRICT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_CER_WARD = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    UPLOAD_FILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HU_FAMILY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_SEND_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_APPROVE_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MODEL_CHANGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_CER_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_SAVE_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_FAMILY_EDIT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_FAMILY_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    RELATIONSHIP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FULLNAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GENDER = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PIT_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SAME_COMPANY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_DEAD = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_DEDUCT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    DEDUCT_FROM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DEDUCT_TO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REGIST_DEDUCT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_HOUSEHOLD = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    ID_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CAREER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NATIONALITY = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_CER_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_CER_PROVINCE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_CER_DISTRICT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_CER_WARD = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    UPLOAD_FILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_FAMILY_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_FILECONTRACT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ID_CONTRACT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    APPEND_TYPEID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTRACT_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    START_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SAL_BASIC = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_PERCENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    UPLOAD_FILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_FILECONTRACT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_FILECONTRACT_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ID_CONTRACT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    APPEND_TYPEID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTRACT_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    START_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_FILECONTRACT_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_FORM_ELEMENT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TYPE_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ORDERS = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_FORM_ELEMENT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_FORM_LIST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ID_MAP = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PARENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ID_ORIGIN = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TEXT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TENANT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_FORM_LIST", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_FROM_ELEMENT",
                columns: table => new
                {
                    ID = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    TYPE_ID = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDERS = table.Column<double>(type: "BINARY_DOUBLE", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_FROM_ELEMENT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_JOB",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME_VN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME_EN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ACTFLG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REQUEST = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PURPOSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PHAN_LOAI_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    JOB_BAND_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    JOB_FAMILY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORDERNUM = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_JOB", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_JOB_BAND",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME_VN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME_EN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEVEL_FROM = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEVEL_TO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TITLE_GROUP_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    OTHER = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_JOB_BAND", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_NATION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_NATION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_ORG_LEVEL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDER_NUM = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_ORG_LEVEL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_ORGANIZATION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TENANT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PARENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COMPANY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    HEAD_POS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORDER_NUM = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MNG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FOUNDATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DISSOLVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAX = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BUSINESS_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BUSINESS_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TAX_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    SHORT_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME_EN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UY_BAN = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    LEVEL_ORG = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    GROUPPROJECT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    ATTACHED_FILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_ORGANIZATION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_POSITION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    GROUP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    JOB_DESC = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TENANT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    JOB_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LM = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ISOWNER = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CSM = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_NONPHYSICAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MASTER = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONCURRENT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_PLAN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    INTERIM = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TYPE_ACTIVITIES = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FILENAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPLOADFILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORKING_TIME = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NAME_EN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_LOCATION = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    HIRING_STATUS = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_TDV = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_NOTOT = table.Column<bool>(type: "NUMBER(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_POSITION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_POSITION_GROUP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_POSITION_GROUP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_PROVINCE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    NATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_PROVINCE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_QUESTION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_MULTIPLE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_ADD_ANSWER = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_QUESTION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_REPORT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PARENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ParentID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_REPORT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HU_REPORT_HU_REPORT_ParentID",
                        column: x => x.ParentID,
                        principalTable: "HU_REPORT",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HU_SALARY_LEVEL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SALARY_RANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_SCALE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PERFORM_BONUS = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    OTHER_BONUS = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EXPIRATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    HOLDING_TIME = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    HOLDING_MONTH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    REGION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_SALARY_LEVEL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_SALARY_RANK",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    LEVEL_START = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SALARY_SCALE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EXPIRATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_SALARY_RANK", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_SALARY_SCALE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EXPIRATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_TABLE_SCORE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_SALARY_SCALE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_SALARY_TYPE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SALARY_TYPE_GROUP = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_SALARY_TYPE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_SETTING_REMIND",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DAY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_SETTING_REMIND", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_TERMINATE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SEND_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    LAST_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TER_REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DECISION_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REASON_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_CAL_SEVERANCE_ALLOWANCE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    AVG_SAL_SIX_MO = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SEVERANCE_ALLOWANCE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    PAYMENT_REMAINING_DAY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    NOTICE_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_TERMINATE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_TITLE_GROUP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_TITLE_GROUP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_WARD",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DISTRICT_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WARD", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_WELFARE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    MONNEY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SENIORITY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DATE_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DATE_END = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SENIORITY_ABOVE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_AUTO_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_CAL_TAX = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    PAYMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PERCENTAGE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    GENDER_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    AGE_FROM = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    AGE_TO = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    WORK_LEAVE_NOPAY_FROM = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    WORK_LEAVE_NOPAY_TO = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTHS_PEND_FROM = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTHS_PEND_TO = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTHS_WORK_IN_YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WELFARE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_WELFARE_AUTO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WELFARE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MONEY = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MONEY_APPROVED = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REMARK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GENDER_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SENIORITY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    COUNT_CHILD = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CONTRACT_TYPE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MONEY_ADJUST = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_PAY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CHILD_AGE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PAY_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CALCULATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DOCUMENT_OFF = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NUMBER_MANUAL = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS50 = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    BIRTH_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WELFARE_AUTO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_WELFARE_CONTRACT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    WELFARE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CONTRACT_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WELFARE_CONTRACT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_WELFARE_MNG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WELFARE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MONEY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PAY_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DECISION_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_TRANSFER = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_CASH = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WELFARE_MNG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_WELFARE_MNG_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WELFARE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MONEY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PAY_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DECISION_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_TRANSFER = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_CASH = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WELFARE_MNG_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_WORK_LOCATION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROVINCE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DISTRICT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WARD = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAX = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WORK_LOCATION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_WORKING",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TENANT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DECISION_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    SALARY_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_SCALE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_RANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_BASIC = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_TOTAL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_PERCENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    IS_CHANGE_SAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORGANIZATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_WAGE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EXPIRE_UPSAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_BHXH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHYT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHTNLD_BNN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHTN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SALARY_SCALE_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_RANK_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_LEVEL_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TAXTABLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT_DCV = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SHORT_TEMP_SALARY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_OBJ_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LABOR_OBJ_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WAGE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_RESPONSIBLE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    BASE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ISSUED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_DATE_DECISION = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SAL_INSU = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_UPSAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REASON_UPSAL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CUR_POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WORKING", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_WORKING_ALLOW",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ALLOWANCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    AMOUNT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    COEFFICIENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    ALLOWANCE_ID1 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT1 = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_DATE1 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE1 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ALLOWANCE_ID2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT2 = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_DATE2 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE2 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ALLOWANCE_ID3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT3 = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_DATE3 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE3 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ALLOWANCE_ID4 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT4 = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_DATE4 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE4 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ALLOWANCE_ID5 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT5 = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_DATE5 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE5 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WORKING_ALLOW", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_WORKING_ALLOW_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    AMOUNT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    ALLOWANCE_ID1 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT1 = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_DATE1 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE1 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ALLOWANCE_ID2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT2 = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_DATE2 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE2 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ALLOWANCE_ID3 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT3 = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_DATE3 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE3 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ALLOWANCE_ID4 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT4 = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_DATE4 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE4 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ALLOWANCE_ID5 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT5 = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_DATE5 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE5 = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WORKING_ALLOW_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_WORKING_BEFORE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COMPANY_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TITLE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    END_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MAIN_DUTY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TER_REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SENIORITY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_NHANUOC = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WORKING_BEFORE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HU_WORKING_BEFORE_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COMPANY_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TITLE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    END_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MAIN_DUTY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TER_REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SENIORITY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_NHANUOC = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WORKING_BEFORE_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_WORKING_HSL_PC_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TENANT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DECISION_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_SCALE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_RANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_BASIC = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_TOTAL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_PERCENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    IS_CHANGE_SAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORGANIZATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_WAGE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EXPIRE_UPSAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_BHXH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHYT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHTNLD_BNN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHTN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SALARY_SCALE_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT_DCV = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SHORT_TEMP_SALARY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    TAXTABLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_RANK_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_LEVEL_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_OBJ_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LABOR_OBJ_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WAGE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_RESPONSIBLE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    BASE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ISSUED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_DATE_DECISION = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SAL_INSU = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_UPSAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REASON_UPSAL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WORKING_HSL_PC_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HU_WORKING_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TENANT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DECISION_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_SCALE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_RANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_BASIC = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_TOTAL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_PERCENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    IS_CHANGE_SAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORGANIZATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_WAGE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EXPIRE_UPSAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_BHXH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHYT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHTNLD_BNN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHTN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SALARY_SCALE_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT_DCV = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SHORT_TEMP_SALARY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    TAXTABLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_RANK_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_LEVEL_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_OBJ_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LABOR_OBJ_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WAGE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_RESPONSIBLE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    BASE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ISSUED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_DATE_DECISION = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SAL_INSU = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_UPSAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REASON_UPSAL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_WORKING_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "HUV_WAGE_MAX",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TENANT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DECISION_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    SALARY_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_SCALE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_RANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_BASIC = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_TOTAL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_PERCENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    IS_CHANGE_SAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORGANIZATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_WAGE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EXPIRE_UPSAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_BHXH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHYT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHTNLD_BNN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHTN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SALARY_SCALE_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_RANK_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_LEVEL_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TAXTABLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT_DCV = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SHORT_TEMP_SALARY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_OBJ_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LABOR_OBJ_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WAGE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_RESPONSIBLE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    BASE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ISSUED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_DATE_DECISION = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SAL_INSU = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_UPSAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REASON_UPSAL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HUV_WAGE_MAX", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HUV_WORKING_MAX_BYTYPE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TENANT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DECISION_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    SALARY_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_SCALE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_RANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_BASIC = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_TOTAL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_PERCENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    IS_CHANGE_SAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORGANIZATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_WAGE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EXPIRE_UPSAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_BHXH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHYT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHTNLD_BNN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHTN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SALARY_SCALE_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_RANK_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_LEVEL_DCV_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TAXTABLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COEFFICIENT_DCV = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SHORT_TEMP_SALARY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    ATTACHMENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_OBJ_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LABOR_OBJ_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WAGE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_RESPONSIBLE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    BASE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ISSUED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_DATE_DECISION = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SAL_INSU = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_UPSAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REASON_UPSAL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HUV_WORKING_MAX_BYTYPE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_ARISING",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    INS_GROUP_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    INS_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    INS_TYPE_CHOOSE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PKEY_REF = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TABLE_REF = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OLD_ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NEW_ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OLD_SAL = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    NEW_SAL = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    OLD_POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NEW_POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SI = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    HI = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    UI = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    AI = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    REASONS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    INS_ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DECLARED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    INS_INFORMATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    INS_SPECIFIED_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_ARISING", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_CHANGE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    SALARY_OLD = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    SALARY_NEW = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CHANGE_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CHANGE_MONTH = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_BHXH = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_BHYT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_BHTN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_BNN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    SALARY_BHXH_BHYT_OLD = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SALARY_BHXH_BHYT_NEW = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SALARY_BHTN_OLD = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SALARY_BHTN_NEW = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DECLARATION_PERIOD = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHYT_REIMBURSEMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ARREARS_FROM_MONTH = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ARREARS_TO_MONTH = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    AR_BHXH_SALARY_DIFFERENCE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    AR_BHYT_SALARY_DIFFERENCE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    AR_BHTN_SALARY_DIFFERENCE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    AR_BHTNLD_BNN_SALARY_DIFFERENCE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    WITHDRAWAL_FROM_MONTH = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WITHDRAWAL_TO_MONTH = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WD_BHXH_SALARY_DIFFERENCE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    WD_BHYT_SALARY_DIFFERENCE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    WD_BHTN_SALARY_DIFFERENCE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    WD_BHTNLD_BNN_SALARY_DIFFERENCE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    UNIT_INSURANCE_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_CHANGE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_DECLARATION_NEW",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FULL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ID_ORG = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ID_POSITION = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ID_UNIT_INSURANCE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ID_TYPE_BDBH = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ID_EMPLOYEE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SALARY_BHXH_BHYT_OLD = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_BHTN_OLD = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_BHXH_BHYT_NEW = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_BHTN_NEW = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CHECKLIST_BHXH = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CHECKLIST_BHYT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CHECKLIST_BHTN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CHECKLIST_BHTNLD_BNN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_DECLARATION_NEW", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_GROUP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_GROUP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_HEALTH_INSURANCE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    INS_CONTRACT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CHECK_BHNT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    FAMILY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DT_CHITRA = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MONEY_INS = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REDUCE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REFUND = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    DATE_RECEIVE_MONEY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EMP_RECEIVE_MONEY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INS_CONTRACT_DE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_HEALTH_INSURANCE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_INFORMATION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SENIORITY_INSURANCE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    BHXH_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHXH_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHXH_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHXH_STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BHXH_SUPPLIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHXH_REIMBURSEMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHXH_GRANT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHXH_DELIVERER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHXH_STORAGE_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHXH_RECEIVER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHXH_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHYT_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHYT_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHYT_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHYT_EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHYT_EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHYT_STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BHYT_WHEREHEALTH_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BHYT_RECEIVED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHYT_RECEIVER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHYT_REIMBURSEMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHTN_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHTN_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHTNLD_BNN_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHTNLD_BNN_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_INFORMATION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_INFORMATION_IMPORT",
                columns: table => new
                {
                    XLSX_USER_ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_EX_CODE = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    XLSX_SESSION = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    XLSX_ROW = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SENIORITY_INSURANCE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    BHXH_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHXH_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHXH_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHXH_STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BHXH_SUPPLIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHXH_REIMBURSEMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHXH_GRANT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHXH_DELIVERER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHXH_STORAGE_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHXH_RECEIVER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHXH_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHYT_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHYT_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHYT_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHYT_EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHYT_EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHYT_STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BHYT_WHEREHEALTH_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BHYT_RECEIVED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHYT_RECEIVER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BHYT_REIMBURSEMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHTN_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHTN_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHTNLD_BNN_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BHTNLD_BNN_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_INSERT_ON = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    XLSX_FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_INFORMATION_IMPORT", x => new { x.XLSX_USER_ID, x.XLSX_EX_CODE, x.XLSX_SESSION, x.XLSX_ROW });
                });

            migrationBuilder.CreateTable(
                name: "INS_LIST_PROGRAM",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_DELETED = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_LIST_PROGRAM", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_REGIMES",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INS_GROUP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CAL_DATE_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TOTAL_DAY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    BENEFITS_LEVELS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_REGIMES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_REGIMES_MNG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REGIME_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    START_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    END_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DAY_CALCULATOR = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ACCUMULATE_DAY = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CHILDREN_NO = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    AVERAGE_SAL_SIX_MONTH = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BHXH_SALARY = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REGIME_SALARY = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SUBSIDY_AMOUNT_CHANGE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SUBSIDY_MONEY_ADVANCE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DECLARE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DATE_CALCULATOR = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    INS_PAY_AMOUNT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PAY_APPROVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    APPROV_DAY_NUM = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    STATUS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_REGIMES_MNG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_REGION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    REGION_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    AREA_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OTHER_LIST_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ACTFLG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MONEY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPRIVED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CEILING_UI = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_REGION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_SPECIFIED_OBJECTS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CHANGE_DAY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SI_HI = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    UI = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SI_COM = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SI_EMP = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    HI_COM = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    HI_EMP = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    UI_COM = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    UI_EMP = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    AI_OAI_COM = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    AI_OAI_EMP = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    RETIRE_MALE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    RETIRE_FEMALE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_SPECIFIED_OBJECTS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_TYPE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    STATUS = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_TYPE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "INS_WHEREHEALTH",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME_VN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ACTFLG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROVINCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    DISTRICT_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INS_WHEREHEALTH", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_ADVANCE_TMP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE_REF = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MONEY = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ADVANCE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_ADVANCE_TMP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_AUTHORITY_TAX_YEAR",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_EMP_REGISTER = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_COM_APPROVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    REASON_REJECT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_AUTHORITY_TAX_YEAR", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_ELEMENT_GROUP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_ELEMENT_GROUP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_IMPORT_MONTHLY_TAX",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TITLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OBJ_SALARY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DATE_CALCULATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DATE_CALCULATE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOBPOSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_CONGDOAN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_KHOAN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CLCHINH8 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TAX21 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TAX24 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TAX25 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    DEDUCT5 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_IMPORT_MONTHLY_TAX", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_KPI_FORMULA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    COL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    FORMULA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FORMULA_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_KPI_FORMULA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_KPI_GROUP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    STATUS = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_KPI_GROUP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_LIST_FUND_SOURCE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    COMPANY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_LIST_FUND_SOURCE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_LISTFUND",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    LISTFUND_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LISTFUND_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COMPANY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_LISTFUND", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_LISTSAL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE_LISTSAL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME_VN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME_EN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DATA_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LIST_KYHIEU_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    THU_TU = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATE_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDER = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_LISTSAL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_LISTSALARIES",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE_SAL = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DATA_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OBJ_SAL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    GROUP_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COL_INDEX = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_VISIBLE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_IMPORT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_QL_TYPE_TN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_FORMULA = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SUM_FORMULA = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_PAYBACK = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_LISTSALARIES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_PAYROLL_FUND",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    AMOUNT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COMPANY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LIST_FUND_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LIST_FUND_SOURCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    APPROVAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_PAYROLL_FUND", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_PAYROLL_TAX_YEAR",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OBJ_SALARY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_LOCK = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_PAYROLL_TAX_YEAR", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_PAYROLLSHEET_SUM",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TITLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OBJ_SALARY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOBPOSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CLCHINH9 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_PAYROLLSHEET_SUM", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_PAYROLLSHEET_SUM_BACKDATE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TITLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OBJ_SALARY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOBPOSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_PAYROLLSHEET_SUM_BACKDATE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_PAYROLLSHEET_SUM_SUB",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TITLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OBJ_SALARY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOBPOSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_PAYROLLSHEET_SUM_SUB", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_PERIOD_TAX",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTHLY_TAX_CALCULATION = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TAX_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CALCULATE_TAX_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CALCULATE_TAX_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_PERIOD_TAX", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_PHASE_ADVANCE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ACTFLG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PHASE_NAME_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MONTH_LBS = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    SENIORITY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    SYMBOL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SYMBOL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NAME_VN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PHASE_DAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_PHASE_ADVANCE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_PHASE_ADVANCE_SYMBOL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PHASE_ADVANCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SYMBOL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_PHASE_ADVANCE_SYMBOL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_SAL_IMPORT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TITLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OBJ_SALARY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOBPOSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_CONGDOAN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_KHOAN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    LUONG_BHXH = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    LUONG_BHTN = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    LUONG_CO_BAN = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CLCHINH8 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    DEDUCT5 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CSUM4 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TAX54 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_SAL_IMPORT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_SAL_IMPORT_ADD",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PHASE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TITLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OBJ_SALARY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOBPOSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_CONGDOAN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_KHOAN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CLCHINH8 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CLCHINH3 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CLCHINH4 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    DEDUCT5 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CL8 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_SAL_IMPORT_ADD", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_SAL_IMPORT_BACKDATE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID_CV = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PERIOD_ADD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TITLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OBJ_SALARY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOBPOSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_CONGDOAN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_KHOAN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    LUONG_BHXH = table.Column<float>(type: "BINARY_FLOAT", nullable: true),
                    LUONG_BHTN = table.Column<float>(type: "BINARY_FLOAT", nullable: true),
                    LUONG_CO_BAN = table.Column<float>(type: "BINARY_FLOAT", nullable: true),
                    TEST2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    AL1 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    AL2 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    AMNL002 = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CLCHINH8 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    DEDUCT5 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CL1 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CL27 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_SAL_IMPORT_BACKDATE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_SAL_IMPORT_TMP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    REF_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    EMPLOYEE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ELEMENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PERIOD_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    VALUES = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TYPE_SAL = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_SAL_IMPORT_TMP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PA_TAX_ANNUAL_IMPORT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    YEAR = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TITLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OBJ_SALARY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOBPOSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_CONGDOAN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_KHOAN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    LUONG_BHXH = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    LUONG_BHTN = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    LUONG_CO_BAN = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    AL2 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    AMNL002 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    KC001 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    AAA = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PPPP = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TTTTT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    YYYYYY = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    DMLK01 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    A002 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    DEDUCT5 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CLCHINH3 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CLCHINH4 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CL1 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CL27 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TAX18 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TAX26 = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_TAX_ANNUAL_IMPORT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PaPayrollsheetTaxs",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TITLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OBJ_SALARY_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOBPOSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaPayrollsheetTaxs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PORTAL_CERTIFICATE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_PRIME = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    TYPE_CERTIFICATE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EFFECT_FROM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TRAIN_FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TRAIN_TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MAJOR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEVEL_TRAIN = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTENT_TRAIN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MARK = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    TYPE_TRAIN = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CODE_CETIFICATE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CLASSIFICATION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REMARK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LEVEL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EFFECT_TO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_SAVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PORTAL_CERTIFICATE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PORTAL_REGISTER_OFF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    DATE_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DATE_END = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TIME_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TIME_END = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORKING_DAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TIME_LATE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TIME_EARLY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TIME_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    RECEIVE_WORKER_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    APPROVE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APPROVE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APPROVE_POS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APPROVE_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APPROVE_DAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    TYPE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TOTAL_DAY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TOTAL_OT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    ID_REGGROUP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_EACH_DAY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PORTAL_REGISTER_OFF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PORTAL_REGISTER_OFF_DETAIL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_REGGROUP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEAVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REGISTER_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    MANUAL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TYPE_OFF = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NUMBER_DAY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SHIFT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PORTAL_REGISTER_OFF_DETAIL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PORTAL_REQUEST_CHANGE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ID_CHANGE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SYS_OTHER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTENT_CHANGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REASON_CHANGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_APPROVE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REASON_DISCARD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SAL_INSU = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PORTAL_REQUEST_CHANGE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PORTAL_ROUTE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PATH = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    VI = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    EN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PORTAL_ROUTE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PT_BLOG_INTERNAL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TITLE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IMG_URL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    THEME_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PT_BLOG_INTERNAL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RC_CANDIDATE_SCANCV",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    FULLNAME_VN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GENDER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SKILL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MOBILEPHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RC_CANDIDATE_SCANCV", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RC_EXAMS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POINT_LADDER = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    COEFFICIENT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    POINT_PASS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EXAMS_ORDER = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_PV = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RC_EXAMS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "REPORT_DATA_STAFF_PROFILE",
                columns: table => new
                {
                    AVATAR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FULL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ORG_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    OTHER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GENDER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ID_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ID_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ID_PLACE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NATIONALITY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WEIGHT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEIGHT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROVINCE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CURRENT_PROVINCE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MOBILE_PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PASS_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PASS_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VISA_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VISA_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PASS_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PASS_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PERMIT_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DOMICILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_CODE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    BIRTH_REGIS_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BLOOD_GROUP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BLOOD_PRESSURE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEALTH_TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEFT_EYE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RIGHT_EYE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEART = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEALTH_NOTES = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CARRER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSURENCE_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HEALTH_CARE_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INS_CARD_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAMILY_MEMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAMILY_POLICY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POLITICAL_THEORY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_STATUS_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TITLE_CONFERRED = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_OF_WORK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRISON_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FAMILY_DETAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UNIONIST_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UNIONIST_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MEMBER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MEMBER_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LIVING_CELL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CARD_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POLITICAL_THEORY_LEVEL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESUME_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VATERANS_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VATERANS_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HIGHEST_MILITARY_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CURRENT_PARTY_COMMITTEE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PARTYTIME_PARTY_COMMITTEE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HOUSEHOLD_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HOUSEHOLD_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MOBILE_PHONE_LAND = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MAIN_INCOME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OTHER_SOURCES = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAND_GRANTED = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_GRANTED_HOUSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TOTAL_AREA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SELF_PURCHASE_LAND = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SELF_BUILD_HOUSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TOTAL_APP_AREA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAND_FOR_PRODUCTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADDITIONAL_INFOMATION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_SAVE_NATION_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_SAVE_NATION_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YELLOW_FLAG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RELATIONS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_GROUP_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YOUTH_GROUP_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DISTRICT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_DISTRICT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MARITAL_STATUS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WARD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_WARD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_NAME_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_BRANCH_NAME_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    QUALIFICATION_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TRAINING_FORM_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEARNING_LEVEL_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NATIVE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RELIGION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EDUCATION_LEVEL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    QUALIFICATION_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    QUALIFICATION_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TRAINING_FORM_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TRAINING_FORM_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE_LEVEL_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE_LEVEL_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE_LEVEL_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRESENTER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRESENTER_PHONE_NUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PRESENTER_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TAX_CODE_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_NAME_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_BRANCH_NAME_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_NO_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LICENSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COMPUTER_SKILL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_OBJECT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INS_WHEREHEALTH = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MEMBER_OFFICAL_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    YOUTH_GROUP_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    YOUTH_SAVE_NATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXAMINATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UNIONIST_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VATERANS_MEMBER_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MEMBER_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ENLISTMENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DISCHARGE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SE_APP_PROCESS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ACTFLG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROCESS_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NUMREQUEST = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_SEND_EMAIL = table.Column<bool>(type: "NUMBER(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_APP_PROCESS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_APP_TEMPLATE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TEMPLATE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TEMPLATE_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TEMPLATE_ORDER = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ACTFLG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TEMPLATE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_APP_TEMPLATE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_APP_TEMPLATE_DTL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TEMPLATE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    APP_LEVEL = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    APP_TYPE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    APP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    INFORM_DATE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    INFORM_EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TITLE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NODE_VIEW = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_APP_TEMPLATE_DTL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_AUTHORIZE_APPROVE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PROCESS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LEVEL_ORDER_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_AUTH_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FROM_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TO_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_PER_REPLACE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_AUTHORIZE_APPROVE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_DOCUMENT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DOCUMENT_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_OBLIGATORY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_PERMISSVE_UPLOAD = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATE_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_DOCUMENT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_DOCUMENT_INFO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DOCUMENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SUB_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_SUBMIT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ATTACHED_FILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_DOCUMENT_INFO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_HR_PROCESS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    START_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDER_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ICON_KEY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_HR_PROCESS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_HR_PROCESS_DATA_MODEL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    START_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDER_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ICON_KEY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_HR_PROCESS_DATA_MODEL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_HR_PROCESS_INSTANCE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    START_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDER_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ICON_KEY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_HR_PROCESS_INSTANCE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_HR_PROCESS_NODE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    START_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDER_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ICON_KEY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_HR_PROCESS_NODE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_HR_PROCESS_TYPE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    START_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDER_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ICON_KEY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_HR_PROCESS_TYPE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_LDAP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    LDAP_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DOMAIN_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    BASE_DN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PORT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_LDAP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_PROCESS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROCESS_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    APPROVED_CONTENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APPROVED_SUCESS_CONTENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOT_APPROVED_CONTENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_NOTI_APPROVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_NOTI_APPROVE_SUCCESS = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_NOTI_NOT_APPROVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    PRO_DESCRIPTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APPROVE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REFUSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADJUSTMENT_PARAM = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_PROCESS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_PROCESS_APPROVE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    APPROVAL_LEVEL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEVEL_ORDER_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    APPROVAL_POSITION = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    SAME_APPROVER = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    PROCESS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DIRECT_MANAGER = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MANAGER_AFFILIATED_DEPARTMENTS = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    MANAGER_SUPERIOR_DEPARTMENTS = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_DIRECT_MNG_OF_DIRECT_MNG = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    APPROVE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REFUSE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CHOOSE_AN_APPROVER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_PROCESS_APPROVE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_PROCESS_APPROVE_POS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PROCESS_APPROVE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_MNG_AFFILIATED_DEPARTMENTS = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_DIRECT_MNG_OF_DIRECT_MNG = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_MNG_SUPERIOR_DEPARTMENTS = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_DIRECT_MANAGER = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_PROCESS_APPROVE_POS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_PROCESS_APPROVE_STATUS",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_REGGROUP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROCESS_TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APP_LEVEL = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    APP_STATUS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PROCESS_APPROVE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    APP_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APP_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROCESS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_APPROVED = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_PROCESS_APPROVE_STATUS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SE_REMINDER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    SYS_OTHERLIST_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    VALUE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SE_REMINDER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SY_AUDITLOG",
                columns: table => new
                {
                    AUDITLOG_ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USER_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EVENT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    EVENT_TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TABLE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RECORD_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COLUMN_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORIGINAL_VALUE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NEW_VALUE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SY_AUDITLOG", x => x.AUDITLOG_ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_ACTION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_ACTION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_CONFIGURATION_COMMON",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    YOUR_MAXIMUM_TURN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PORTAL_PORT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    APPLICATION_PORT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MINIMUM_LENGTH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_UPPERCASE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_NUMBER = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_LOWERCASE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SPECIAL_CHAR = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_CONFIGURATION_COMMON", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_CONTRACT_TYPE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PERIOD = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DAY_NOTICE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_LEAVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_CONTRACT_TYPE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_FORM_LIST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ID_MAP = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PARENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ID_ORIGIN = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TEXT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_FORM_LIST", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_FUNCTION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    MODULE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    GROUP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PATH = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PATH_FULL_MATCH = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    ROOT_ONLY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_FUNCTION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_FUNCTION_ACTION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    FUNCTION_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ACTION_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_FUNCTION_ACTION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_FUNCTION_GROUP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APPLICATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_FUNCTION_GROUP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_FUNCTION_IGNORE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PATH = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_FUNCTION_IGNORE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_GROUP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ADMIN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_SYSTEM = table.Column<bool>(type: "NUMBER(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_GROUP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_GROUP_FUNCTION_ACTION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    GROUP_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    FUNCTION_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ACTION_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_GROUP_FUNCTION_ACTION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_KPI",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    KPI_GROUP_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    UNIT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_REAL_VALUE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_PAY_SALARY = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_IMPORT_KPI = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_SYSTEM = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_KPI", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_KPI_GROUP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_KPI_GROUP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_LANGUAGE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    KEY = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    VI = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    EN = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_LANGUAGE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_MENU",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ROOT_ONLY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    ORDER_NUMBER = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ICON_FONT_FAMILY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ICON_CLASS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SYS_MENU_SERVICE_METHOD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    URL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PARENT = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FUNCTION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    INACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_MENU", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_MODULE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    APPLICATION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PRICE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_MODULE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_MUTATION_LOG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    SYS_FUNCTION_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    SYS_ACTION_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    BEFORE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    AFTER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    USERNAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_MUTATION_LOG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_OTHER_LIST",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EFFECTIVE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_OTHER_LIST", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_OTHER_LIST_FIX",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_OTHER_LIST_FIX", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_OTHER_LIST_TYPE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_SYSTEM = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_OTHER_LIST_TYPE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_PA_ELEMENT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    GROUP_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    AREA_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    IS_SYSTEM = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATA_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_PA_ELEMENT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_PA_FORMULA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    AREA_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SALARY_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FORMULA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FORMULA_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_PA_FORMULA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_PERMISSION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_PERMISSION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_REFRESH_TOKEN",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TOKEN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    EXPIRES = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    CREATED = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    CREATED_BY_IP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    REVOKED = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REVOKED_BY_IP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REPLACED_BY_TOKEN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REASON_REVOKED = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_REFRESH_TOKEN", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_SALARY_TYPE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    AREA_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_SALARY_TYPE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_SETTING_MAP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RADIUS = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    LAT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LNG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ZOOM = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CENTER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BSSID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    QRCODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_SETTING_MAP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_TMP_SORT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TMP_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    REF_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_TMP_SORT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_USER",
                columns: table => new
                {
                    ID = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    DISCRIMINATOR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GROUP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FULLNAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ADMIN = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_ROOT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    AVATAR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    USERNAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NORMALIZEDUSERNAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NORMALIZEDEMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMAILCONFIRMED = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_LOCK = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    PASSWORDHASH = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SECURITYSTAMP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONCURRENCYSTAMP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PHONENUMBER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PHONENUMBERCONFIRMED = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    TWOFACTORENABLED = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    LOCKOUTEND = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: true),
                    LOCKOUTENABLED = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ACCESSFAILEDCOUNT = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_FIRST_LOGIN = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_WEBAPP = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_LDAP = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CHANGE_PASSWORD_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_USER_FUNCTION_ACTION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USER_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    FUNCTION_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ACTION_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USER_FUNCTION_ACTION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_USER_GROUP_ORG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    GROUP_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USER_GROUP_ORG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_USER_ORG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USER_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USER_ORG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SYS_USER_PERMISSION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USER_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FUNCTION_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PERMISSION_STRING = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_USER_PERMISSION", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysConfigs",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TYPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysConfigs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "THEME_BLOG",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IMG_URL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COLOR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_THEME_BLOG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TMP_HU_CONTRACT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    REF_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DATE_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DATE_END = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CONTRACT_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTRACT_TYPE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTRACT_TYPE_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SAL_BASIC = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_TOTAL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_PERCENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    STATUS_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMP_HU_CONTRACT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TMP_HU_WORKING",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    REF_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POS_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DECISION_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TYPE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_TYPE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SALARY_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SALARY_LEVEL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SAL_BASIC = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    COEFFICIENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_TOTAL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_PERCENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    STATUS_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMP_HU_WORKING", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TMP_INS_CHANGE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    REF_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TYPE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CHANGE_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CHANGE_MONTH = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    SALARY_OLD = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    SALARY_NEW = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    IS_BHXH = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHYT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BHTN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_BNN = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMP_INS_CHANGE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TR_CENTER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE_CENTER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME_CENTER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TRAINING_FIELD = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    REPRESENTATIVE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTACT_PERSON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PHONE_CONTACT_PERSON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WEBSITE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ATTACHED_FILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TR_CENTER", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TR_COURSE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    COURSE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COURSE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COURSE_DATE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    COSTS = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TR_COURSE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TR_PLAN",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    YEAR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    START_DATE_PLAN = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    END_DATE_PLAN = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    START_DATE_REAL = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    END_DATE_REAL = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PERSON_NUM_REAL = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PERSON_NUM_PLAN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EXPECTED_COST = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ACTUAL_COST = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    COURSE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTENT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FORM_TRAINING = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ADDRESS_TRAINING = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CENTER_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FILENAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TR_PLAN", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UniqueIndexes",
                columns: table => new
                {
                    TableView = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ObjectType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ConstraintType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ConstraintName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Columns = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    IndexName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    IndexType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AD_BOOKING",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMP_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ROOM_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    BOOKING_DAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    HOUR_FROM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    HOUR_TO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    APPROVED_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    APPROVED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    APPROVED_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ROOMID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AD_BOOKING", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AD_BOOKING_AD_ROOM_ROOMID",
                        column: x => x.ROOMID,
                        principalTable: "AD_ROOM",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    RoleId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ClaimValue = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ClaimValue = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    RoleId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Value = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AT_SALARY_PERIOD_DTL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_STANDARD = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    STANDARD_TIME = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PERIODID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_SALARY_PERIOD_DTL", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AT_SALARY_PERIOD_DTL_AT_SALARY_PERIOD_PERIODID",
                        column: x => x.PERIODID,
                        principalTable: "AT_SALARY_PERIOD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AT_TIMESHEET_LOCK",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ORG_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PERIODID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_TIMESHEET_LOCK", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AT_TIMESHEET_LOCK_AT_SALARY_PERIOD_PERIODID",
                        column: x => x.PERIODID,
                        principalTable: "AT_SALARY_PERIOD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PA_KPI_SALARY_DETAIL_TMP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    KPI_TARGET_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    REAL_VALUE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    START_VALUE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EQUAL_VALUE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    PERIODID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_KPI_SALARY_DETAIL_TMP", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PA_KPI_SALARY_DETAIL_TMP_AT_SALARY_PERIOD_PERIODID",
                        column: x => x.PERIODID,
                        principalTable: "AT_SALARY_PERIOD",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AT_WORKSIGN_DUTY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKINGDAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    SHIFT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SHIFTID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_WORKSIGN_DUTY", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AT_WORKSIGN_DUTY_AT_SHIFT_SHIFTID",
                        column: x => x.SHIFTID,
                        principalTable: "AT_SHIFT",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CSS_THEME_VAR",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CSS_THEME_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CSS_VAR_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    VALUE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CSS_THEMEID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CSS_VARID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CSS_THEME_VAR", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CSS_THEME_VAR_CSS_THEME_CSS_THEMEID",
                        column: x => x.CSS_THEMEID,
                        principalTable: "CSS_THEME",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CSS_THEME_VAR_CSS_VAR_CSS_VARID",
                        column: x => x.CSS_VARID,
                        principalTable: "CSS_VAR",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HU_EMPLOYEE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSURENCE_AREA_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTRACT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTRACT_EXPIRED = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CONTRACT_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    RESIDENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LAST_WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DIRECT_MANAGER_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_OBJECT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LABOR_OBJECT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TER_EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TER_LAST_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SALARY_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SENIORITY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SAL_TOTAL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_BASIC = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    SAL_RATE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    DAY_OF = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    WORK_STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    STATUS_DETAIL_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORK_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    JOIN_DATE_STATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    STAFF_RANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    INSURENCE_AREA = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_NOT_CONTRACT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EMPLOYEE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HU_EMPLOYEE_HU_EMPLOYEE_CV_PROFILE_ID",
                        column: x => x.PROFILE_ID,
                        principalTable: "HU_EMPLOYEE_CV",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HU_JOB_FUNCTION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME_EN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODIFIED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    MODIFIED_LOG = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PARENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    JOB_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    FUNCTION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    JOBID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PARENTID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_JOB_FUNCTION", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HU_JOB_FUNCTION_HU_JOB_FUNCTION_PARENTID",
                        column: x => x.PARENTID,
                        principalTable: "HU_JOB_FUNCTION",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HU_JOB_FUNCTION_HU_JOB_JOBID",
                        column: x => x.JOBID,
                        principalTable: "HU_JOB",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PA_CALCULATE_LOCK",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORGID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PERIODID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_CALCULATE_LOCK", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PA_CALCULATE_LOCK_AT_SALARY_PERIOD_PERIODID",
                        column: x => x.PERIODID,
                        principalTable: "AT_SALARY_PERIOD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PA_CALCULATE_LOCK_HU_ORGANIZATION_ORGID",
                        column: x => x.ORGID,
                        principalTable: "HU_ORGANIZATION",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PA_KPI_LOCK",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORGID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PERIODID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_KPI_LOCK", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PA_KPI_LOCK_AT_SALARY_PERIOD_PERIODID",
                        column: x => x.PERIODID,
                        principalTable: "AT_SALARY_PERIOD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PA_KPI_LOCK_HU_ORGANIZATION_ORGID",
                        column: x => x.ORGID,
                        principalTable: "HU_ORGANIZATION",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HU_JOB_DESCRIPTION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    JOB_TARGET_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    JOB_TARGET_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    JOB_TARGET_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    JOB_TARGET_4 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    JOB_TARGET_5 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    JOB_TARGET_6 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TDCM = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    MAJOR = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_EXP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LANGUAGE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    COMPUTER = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SUPPORT_SKILL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INTERNAL_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INTERNAL_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INTERNAL_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OUTSIDE_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OUTSIDE_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OUTSIDE_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPONSIBILITY_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPONSIBILITY_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPONSIBILITY_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPONSIBILITY_4 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPONSIBILITY_5 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DETAIL_RESPONSIBILITY_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DETAIL_RESPONSIBILITY_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DETAIL_RESPONSIBILITY_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DETAIL_RESPONSIBILITY_4 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DETAIL_RESPONSIBILITY_5 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OUT_RESULT_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OUT_RESULT_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OUT_RESULT_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OUT_RESULT_4 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OUT_RESULT_5 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PERMISSION_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PERMISSION_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PERMISSION_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PERMISSION_4 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PERMISSION_5 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PERMISSION_6 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FILE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    UPLOAD_FILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    POSITIONID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_JOB_DESCRIPTION", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HU_JOB_DESCRIPTION_HU_POSITION_POSITIONID",
                        column: x => x.POSITIONID,
                        principalTable: "HU_POSITION",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PA_FORMULA",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    COL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    SALARY_TYPE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    FORMULA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    FORMULA_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_DAILY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SALARY_TYPEID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_FORMULA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PA_FORMULA_HU_SALARY_TYPE_SALARY_TYPEID",
                        column: x => x.SALARY_TYPEID,
                        principalTable: "HU_SALARY_TYPE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PA_ELEMENT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    GROUP_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    IS_SYSTEM = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATA_TYPE = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    GROUPID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_ELEMENT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PA_ELEMENT_PA_ELEMENT_GROUP_GROUPID",
                        column: x => x.GROUPID,
                        principalTable: "PA_ELEMENT_GROUP",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PA_KPI_TARGET",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    KPI_GROUP_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    UNIT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    COL_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    COL_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MAX_VALUE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_REAL_VALUE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_PAY_SALARY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_IMPORT_KPI = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_SYSTEM = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    KPI_GROUPID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_KPI_TARGET", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PA_KPI_TARGET_PA_KPI_GROUP_KPI_GROUPID",
                        column: x => x.KPI_GROUPID,
                        principalTable: "PA_KPI_GROUP",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HU_CONTRACT_TYPE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PERIOD = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DAY_NOTICE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_LEAVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    IS_BHXH = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_BHYT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_BHTN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_BHTNLD_BNN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_CONTRACT_TYPE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HU_CONTRACT_TYPE_SYS_CONTRACT_TYPE_TYPE_ID",
                        column: x => x.TYPE_ID,
                        principalTable: "SYS_CONTRACT_TYPE",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SYS_GROUP_PERMISSION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    GROUP_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    FUNCTION_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PERMISSION_STRING = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    FUNCTIONID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    GROUPID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_GROUP_PERMISSION", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_GROUP_PERMISSION_SYS_FUNCTION_FUNCTIONID",
                        column: x => x.FUNCTIONID,
                        principalTable: "SYS_FUNCTION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_GROUP_PERMISSION_SYS_GROUP_GROUPID",
                        column: x => x.GROUPID,
                        principalTable: "SYS_GROUP",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HU_POS_PAPER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    POS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PAPER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PAPERID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    POSID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_POS_PAPER", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HU_POS_PAPER_HU_POSITION_POSID",
                        column: x => x.POSID,
                        principalTable: "HU_POSITION",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HU_POS_PAPER_SYS_OTHER_LIST_PAPERID",
                        column: x => x.PAPERID,
                        principalTable: "SYS_OTHER_LIST",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SYS_SALARY_STRUCTURE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    AREA_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    SALARY_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ELEMENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    IS_VISIBLE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_CALCULATE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_IMPORT = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ELEMENTID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    SALARY_TYPEID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_SALARY_STRUCTURE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SYS_SALARY_STRUCTURE_SYS_PA_ELEMENT_ELEMENTID",
                        column: x => x.ELEMENTID,
                        principalTable: "SYS_PA_ELEMENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_SALARY_STRUCTURE_SYS_SALARY_TYPE_SALARY_TYPEID",
                        column: x => x.SALARY_TYPEID,
                        principalTable: "SYS_SALARY_TYPE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AT_REGISTER_OFF",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    DATE_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATE_END = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TIME_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TIME_END = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORKING_DAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TIME_LATE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TIME_EARLY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TIMETYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TYPE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    STATUS_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    APPROVE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    APPROVE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APPROVE_POS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APPROVE_DAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    APPROVE_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EMPLOYEEID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    TIMETYPEID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_REGISTER_OFF", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AT_REGISTER_OFF_AT_TIME_TYPE_TIMETYPEID",
                        column: x => x.TIMETYPEID,
                        principalTable: "AT_TIME_TYPE",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AT_REGISTER_OFF_HU_EMPLOYEE_EMPLOYEEID",
                        column: x => x.EMPLOYEEID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AT_SWIPE_DATA_TMP",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TENANT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    REF_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMP_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TIME_POINT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    LATITUDE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LONGITUDE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MODEL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IMAGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MAC = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OPERATING_SYSTEM = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OPERATING_VERSION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WIFI_IP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BSS_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_SWIPE_DATA_TMP", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AT_SWIPE_DATA_TMP_HU_EMPLOYEE_EMPID",
                        column: x => x.EMPID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AT_TIME_LATE_EARLY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DATE_START = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DATE_END = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TIME_LATE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TIME_EARLY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    STATUS_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EMPLOYEEID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_TIME_LATE_EARLY", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AT_TIME_LATE_EARLY_HU_EMPLOYEE_EMPLOYEEID",
                        column: x => x.EMPLOYEEID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AT_TIMESHEET_DAILY",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    WORKINGDAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TIMETYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    OT_TIME = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    OT_TIME_NIGHT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_REGISTER_OFF = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_REGISTER_LATE_EARLY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_HOLIDAY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EMPLOYEEID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PERIODID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    TIMETYPEID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_TIMESHEET_DAILY", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AT_TIMESHEET_DAILY_AT_SALARY_PERIOD_PERIODID",
                        column: x => x.PERIODID,
                        principalTable: "AT_SALARY_PERIOD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AT_TIMESHEET_DAILY_AT_TIME_TYPE_TIMETYPEID",
                        column: x => x.TIMETYPEID,
                        principalTable: "AT_TIME_TYPE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AT_TIMESHEET_DAILY_HU_EMPLOYEE_EMPLOYEEID",
                        column: x => x.EMPLOYEEID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AT_TIMESHEET_MACHINE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TENANT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKINGDAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TIMETYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TIME_POINT1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TIME_POINT4 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TIME_POINT_OT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OT_START = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OT_END = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    OT_LATE_IN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    OT_EARLY_OUT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    OT_TIME = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    OT_TIME_NIGHT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_REGISTER_OFF = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_REGISTER_LATE_EARLY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_HOLIDAY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    LATE_IN = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EARLY_OUT = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    HOURS_START = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HOURS_STOP = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_EDIT_IN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_EDIT_OUT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MORNING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    AFTERNOON_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEEID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_TIMESHEET_MACHINE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AT_TIMESHEET_MACHINE_HU_EMPLOYEE_EMPLOYEEID",
                        column: x => x.EMPLOYEEID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AT_TIMESHEET_MACHINE_EDIT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TENANT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKINGDAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    TIME_POINT1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TIME_POINT4 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_EDIT_IN = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_EDIT_OUT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMPLOYEEID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PERIODID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_TIMESHEET_MACHINE_EDIT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AT_TIMESHEET_MACHINE_EDIT_AT_SALARY_PERIOD_PERIODID",
                        column: x => x.PERIODID,
                        principalTable: "AT_SALARY_PERIOD",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AT_TIMESHEET_MACHINE_EDIT_HU_EMPLOYEE_EMPLOYEEID",
                        column: x => x.EMPLOYEEID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AT_TIMESHEET_MONTHLY_DTL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    FULLDAY = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    WORKING_DAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_CD = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_P = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_KL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_NB = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_L = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_X = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_CT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_VR = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_TS = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_H = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_XL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_OFF = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING__ = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_LPAY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_PAY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_N = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    WORKING_O = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    OT_NL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    OT_DNL = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    OT_NN = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    OT_DNN = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    OT_NT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    OT_DNT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EMPLOYEEID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ORGID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    PERIODID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_TIMESHEET_MONTHLY_DTL", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AT_TIMESHEET_MONTHLY_DTL_AT_SALARY_PERIOD_PERIODID",
                        column: x => x.PERIODID,
                        principalTable: "AT_SALARY_PERIOD",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AT_TIMESHEET_MONTHLY_DTL_HU_EMPLOYEE_EMPLOYEEID",
                        column: x => x.EMPLOYEEID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AT_TIMESHEET_MONTHLY_DTL_HU_ORGANIZATION_ORGID",
                        column: x => x.ORGID,
                        principalTable: "HU_ORGANIZATION",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HU_ANSWER_USER",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ANSWER_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    EMP_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ANSWERID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    EMPID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_ANSWER_USER", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HU_ANSWER_USER_HU_ANSWER_ANSWERID",
                        column: x => x.ANSWERID,
                        principalTable: "HU_ANSWER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HU_ANSWER_USER_HU_EMPLOYEE_EMPID",
                        column: x => x.EMPID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PA_ADVANCE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PERIOD_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MONEY = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ADVANCE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EMPLOYEEID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    SIGNID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_ADVANCE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PA_ADVANCE_HU_EMPLOYEE_EMPLOYEEID",
                        column: x => x.EMPLOYEEID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PA_ADVANCE_HU_EMPLOYEE_SIGNID",
                        column: x => x.SIGNID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PA_SALARY_PAYCHECK",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    SALARY_TYPE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ELEMENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_VISIBLE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ELEMENTID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    SALARY_TYPEID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_SALARY_PAYCHECK", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PA_SALARY_PAYCHECK_HU_SALARY_TYPE_SALARY_TYPEID",
                        column: x => x.SALARY_TYPEID,
                        principalTable: "HU_SALARY_TYPE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PA_SALARY_PAYCHECK_PA_ELEMENT_ELEMENTID",
                        column: x => x.ELEMENTID,
                        principalTable: "PA_ELEMENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PA_SALARY_STRUCTURE",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    SALARY_TYPE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ELEMENT_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    IS_VISIBLE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_CALCULATE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_IMPORT = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_SUM = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_CHANGE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    ORDERS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ELEMENTID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    SALARY_TYPEID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_SALARY_STRUCTURE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PA_SALARY_STRUCTURE_HU_SALARY_TYPE_SALARY_TYPEID",
                        column: x => x.SALARY_TYPEID,
                        principalTable: "HU_SALARY_TYPE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PA_SALARY_STRUCTURE_PA_ELEMENT_ELEMENTID",
                        column: x => x.ELEMENTID,
                        principalTable: "PA_ELEMENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PA_KPI_POSITION",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    KPI_TARGET_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    KPI_TARGETID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    POSITIONID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_KPI_POSITION", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PA_KPI_POSITION_HU_POSITION_POSITIONID",
                        column: x => x.POSITIONID,
                        principalTable: "HU_POSITION",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PA_KPI_POSITION_PA_KPI_TARGET_KPI_TARGETID",
                        column: x => x.KPI_TARGETID,
                        principalTable: "PA_KPI_TARGET",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PA_KPI_SALARY_DETAIL",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERIOD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    KPI_TARGET_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    REAL_VALUE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    START_VALUE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    EQUAL_VALUE = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    KPI_SALARY = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: true),
                    IS_PAY_SALARY = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EMPLOYEEID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    KPI_TARGETID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PERIODID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PA_KPI_SALARY_DETAIL", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PA_KPI_SALARY_DETAIL_AT_SALARY_PERIOD_PERIODID",
                        column: x => x.PERIODID,
                        principalTable: "AT_SALARY_PERIOD",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PA_KPI_SALARY_DETAIL_HU_EMPLOYEE_EMPLOYEEID",
                        column: x => x.EMPLOYEEID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PA_KPI_SALARY_DETAIL_PA_KPI_TARGET_KPI_TARGETID",
                        column: x => x.KPI_TARGETID,
                        principalTable: "PA_KPI_TARGET",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HU_CONTRACT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTRACT_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CONTRACT_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    START_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_PROFILE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGN_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    SIGNER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGNER_POSITION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SIGN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SAL_BASIC = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    SAL_PERCENT = table.Column<decimal>(type: "DECIMAL(18,9)", precision: 18, scale: 9, nullable: false),
                    NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATUS_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPLOAD_FILE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IS_RECEIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    LIQUIDATION_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    LIQUIDATION_REASON = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_CONTRACT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HU_CONTRACT_HU_CONTRACT_TYPE_CONTRACT_TYPE_ID",
                        column: x => x.CONTRACT_TYPE_ID,
                        principalTable: "HU_CONTRACT_TYPE",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HU_CONTRACT_HU_EMPLOYEE_EMPLOYEE_ID",
                        column: x => x.EMPLOYEE_ID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HU_CONTRACT_HU_EMPLOYEE_SIGN_ID",
                        column: x => x.SIGN_ID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HU_CONTRACT_HU_WORKING_WORKING_ID",
                        column: x => x.WORKING_ID,
                        principalTable: "HU_WORKING",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AT_APPROVED",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    REGISTER_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    EMP_RES_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    APPROVE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    APPROVE_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APPROVE_POS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    APPROVE_DAY = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    APPROVE_NOTE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TYPE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_REG = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    STATUS_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_READ = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TIME_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    REGISTERID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    TimeTypeID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    employeeID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AT_APPROVED", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AT_APPROVED_AT_REGISTER_OFF_REGISTERID",
                        column: x => x.REGISTERID,
                        principalTable: "AT_REGISTER_OFF",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AT_APPROVED_AT_TIME_TYPE_TimeTypeID",
                        column: x => x.TimeTypeID,
                        principalTable: "AT_TIME_TYPE",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AT_APPROVED_HU_EMPLOYEE_employeeID",
                        column: x => x.employeeID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HU_EMPLOYEE_EDIT",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EMPLOYEE_ID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FIRST_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FULLNAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ORG_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTRACT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    CONTRACT_TYPE_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    POSITION_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DIRECT_MANAGER = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    LAST_WORKING_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DIRECT_MANAGER_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    GENDER_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ID_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ID_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RELIGION_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NATIVE_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NATIONALITY_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BIRTH_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_STATUS_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PROVINCE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    DISTRICT_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    WARD_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TER_EFFECT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ITIME_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    JOIN_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    SALARY_TYPE_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    TAX_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MOBILE_PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MARITAL_STATUS_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PASS_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PASS_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PASS_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PASS_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VISA_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VISA_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VISA_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SENIORITY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    STATUS = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BIRTH_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PERMIT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT_EXPIRE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_PERMIT_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    WORK_SCOPE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    WORK_PLACE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTACT_PER = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CONTACT_PER_PHONE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_ID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    BANK_BRANCH = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    BANK_NO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOL_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SCHOOLNAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TRAININGFORMNAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LEARNINGLEVELNAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    QUALIFICATION_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    QUALIFICATIONID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    LANGUAGE_MARK = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TRAINING_FORM_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    LEARNING_LEVEL_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    LANGUAGE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESIDENT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SAL_TOTAL = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ID_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    CUR_ADDRESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CUR_WARD_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CUR_DISTRICT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CUR_PROVINCE_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UPDATED_BY = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    STAFF_RANK_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_SEND_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_APPROVED_PORTAL = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CONTRACTID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    EMPLOYEEID = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ORGID = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    POSITIONID = table.Column<long>(type: "NUMBER(19)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HU_EMPLOYEE_EDIT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HU_EMPLOYEE_EDIT_HU_CONTRACT_CONTRACTID",
                        column: x => x.CONTRACTID,
                        principalTable: "HU_CONTRACT",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HU_EMPLOYEE_EDIT_HU_EMPLOYEE_EMPLOYEEID",
                        column: x => x.EMPLOYEEID,
                        principalTable: "HU_EMPLOYEE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HU_EMPLOYEE_EDIT_HU_ORGANIZATION_ORGID",
                        column: x => x.ORGID,
                        principalTable: "HU_ORGANIZATION",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HU_EMPLOYEE_EDIT_HU_POSITION_POSITIONID",
                        column: x => x.POSITIONID,
                        principalTable: "HU_POSITION",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AD_BOOKING_ROOMID",
                table: "AD_BOOKING",
                column: "ROOMID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "\"NormalizedName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "\"NormalizedUserName\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AT_APPROVED_employeeID",
                table: "AT_APPROVED",
                column: "employeeID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_APPROVED_REGISTERID",
                table: "AT_APPROVED",
                column: "REGISTERID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_APPROVED_TimeTypeID",
                table: "AT_APPROVED",
                column: "TimeTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_REGISTER_OFF_EMPLOYEEID",
                table: "AT_REGISTER_OFF",
                column: "EMPLOYEEID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_REGISTER_OFF_TIMETYPEID",
                table: "AT_REGISTER_OFF",
                column: "TIMETYPEID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_SALARY_PERIOD_DTL_PERIODID",
                table: "AT_SALARY_PERIOD_DTL",
                column: "PERIODID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_SWIPE_DATA_TMP_EMPID",
                table: "AT_SWIPE_DATA_TMP",
                column: "EMPID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_TIME_LATE_EARLY_EMPLOYEEID",
                table: "AT_TIME_LATE_EARLY",
                column: "EMPLOYEEID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_TIMESHEET_DAILY_EMPLOYEEID",
                table: "AT_TIMESHEET_DAILY",
                column: "EMPLOYEEID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_TIMESHEET_DAILY_PERIODID",
                table: "AT_TIMESHEET_DAILY",
                column: "PERIODID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_TIMESHEET_DAILY_TIMETYPEID",
                table: "AT_TIMESHEET_DAILY",
                column: "TIMETYPEID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_TIMESHEET_LOCK_PERIODID",
                table: "AT_TIMESHEET_LOCK",
                column: "PERIODID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_TIMESHEET_MACHINE_EMPLOYEEID",
                table: "AT_TIMESHEET_MACHINE",
                column: "EMPLOYEEID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_TIMESHEET_MACHINE_EDIT_EMPLOYEEID",
                table: "AT_TIMESHEET_MACHINE_EDIT",
                column: "EMPLOYEEID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_TIMESHEET_MACHINE_EDIT_PERIODID",
                table: "AT_TIMESHEET_MACHINE_EDIT",
                column: "PERIODID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_TIMESHEET_MONTHLY_DTL_EMPLOYEEID",
                table: "AT_TIMESHEET_MONTHLY_DTL",
                column: "EMPLOYEEID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_TIMESHEET_MONTHLY_DTL_ORGID",
                table: "AT_TIMESHEET_MONTHLY_DTL",
                column: "ORGID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_TIMESHEET_MONTHLY_DTL_PERIODID",
                table: "AT_TIMESHEET_MONTHLY_DTL",
                column: "PERIODID");

            migrationBuilder.CreateIndex(
                name: "IX_AT_WORKSIGN_DUTY_SHIFTID",
                table: "AT_WORKSIGN_DUTY",
                column: "SHIFTID");

            migrationBuilder.CreateIndex(
                name: "IX_CSS_THEME_VAR_CSS_THEMEID",
                table: "CSS_THEME_VAR",
                column: "CSS_THEMEID");

            migrationBuilder.CreateIndex(
                name: "IX_CSS_THEME_VAR_CSS_VARID",
                table: "CSS_THEME_VAR",
                column: "CSS_VARID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_ANSWER_USER_ANSWERID",
                table: "HU_ANSWER_USER",
                column: "ANSWERID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_ANSWER_USER_EMPID",
                table: "HU_ANSWER_USER",
                column: "EMPID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_CONTRACT_CONTRACT_TYPE_ID",
                table: "HU_CONTRACT",
                column: "CONTRACT_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_CONTRACT_EMPLOYEE_ID",
                table: "HU_CONTRACT",
                column: "EMPLOYEE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_CONTRACT_SIGN_ID",
                table: "HU_CONTRACT",
                column: "SIGN_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_CONTRACT_WORKING_ID",
                table: "HU_CONTRACT",
                column: "WORKING_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_CONTRACT_TYPE_TYPE_ID",
                table: "HU_CONTRACT_TYPE",
                column: "TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_EMPLOYEE_PROFILE_ID",
                table: "HU_EMPLOYEE",
                column: "PROFILE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_EMPLOYEE_EDIT_CONTRACTID",
                table: "HU_EMPLOYEE_EDIT",
                column: "CONTRACTID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_EMPLOYEE_EDIT_EMPLOYEEID",
                table: "HU_EMPLOYEE_EDIT",
                column: "EMPLOYEEID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_EMPLOYEE_EDIT_ORGID",
                table: "HU_EMPLOYEE_EDIT",
                column: "ORGID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_EMPLOYEE_EDIT_POSITIONID",
                table: "HU_EMPLOYEE_EDIT",
                column: "POSITIONID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_JOB_DESCRIPTION_POSITIONID",
                table: "HU_JOB_DESCRIPTION",
                column: "POSITIONID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_JOB_FUNCTION_JOBID",
                table: "HU_JOB_FUNCTION",
                column: "JOBID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_JOB_FUNCTION_PARENTID",
                table: "HU_JOB_FUNCTION",
                column: "PARENTID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_POS_PAPER_PAPERID",
                table: "HU_POS_PAPER",
                column: "PAPERID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_POS_PAPER_POSID",
                table: "HU_POS_PAPER",
                column: "POSID");

            migrationBuilder.CreateIndex(
                name: "IX_HU_REPORT_ParentID",
                table: "HU_REPORT",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_ADVANCE_EMPLOYEEID",
                table: "PA_ADVANCE",
                column: "EMPLOYEEID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_ADVANCE_SIGNID",
                table: "PA_ADVANCE",
                column: "SIGNID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_CALCULATE_LOCK_ORGID",
                table: "PA_CALCULATE_LOCK",
                column: "ORGID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_CALCULATE_LOCK_PERIODID",
                table: "PA_CALCULATE_LOCK",
                column: "PERIODID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_ELEMENT_GROUPID",
                table: "PA_ELEMENT",
                column: "GROUPID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_FORMULA_SALARY_TYPEID",
                table: "PA_FORMULA",
                column: "SALARY_TYPEID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_KPI_LOCK_ORGID",
                table: "PA_KPI_LOCK",
                column: "ORGID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_KPI_LOCK_PERIODID",
                table: "PA_KPI_LOCK",
                column: "PERIODID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_KPI_POSITION_KPI_TARGETID",
                table: "PA_KPI_POSITION",
                column: "KPI_TARGETID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_KPI_POSITION_POSITIONID",
                table: "PA_KPI_POSITION",
                column: "POSITIONID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_KPI_SALARY_DETAIL_EMPLOYEEID",
                table: "PA_KPI_SALARY_DETAIL",
                column: "EMPLOYEEID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_KPI_SALARY_DETAIL_KPI_TARGETID",
                table: "PA_KPI_SALARY_DETAIL",
                column: "KPI_TARGETID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_KPI_SALARY_DETAIL_PERIODID",
                table: "PA_KPI_SALARY_DETAIL",
                column: "PERIODID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_KPI_SALARY_DETAIL_TMP_PERIODID",
                table: "PA_KPI_SALARY_DETAIL_TMP",
                column: "PERIODID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_KPI_TARGET_KPI_GROUPID",
                table: "PA_KPI_TARGET",
                column: "KPI_GROUPID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_SALARY_PAYCHECK_ELEMENTID",
                table: "PA_SALARY_PAYCHECK",
                column: "ELEMENTID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_SALARY_PAYCHECK_SALARY_TYPEID",
                table: "PA_SALARY_PAYCHECK",
                column: "SALARY_TYPEID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_SALARY_STRUCTURE_ELEMENTID",
                table: "PA_SALARY_STRUCTURE",
                column: "ELEMENTID");

            migrationBuilder.CreateIndex(
                name: "IX_PA_SALARY_STRUCTURE_SALARY_TYPEID",
                table: "PA_SALARY_STRUCTURE",
                column: "SALARY_TYPEID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_GROUP_PERMISSION_FUNCTIONID",
                table: "SYS_GROUP_PERMISSION",
                column: "FUNCTIONID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_GROUP_PERMISSION_GROUPID",
                table: "SYS_GROUP_PERMISSION",
                column: "GROUPID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_SALARY_STRUCTURE_ELEMENTID",
                table: "SYS_SALARY_STRUCTURE",
                column: "ELEMENTID");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_SALARY_STRUCTURE_SALARY_TYPEID",
                table: "SYS_SALARY_STRUCTURE",
                column: "SALARY_TYPEID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AD_BOOKING");

            migrationBuilder.DropTable(
                name: "AD_PROGRAMS");

            migrationBuilder.DropTable(
                name: "AD_REQUESTS");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AT_APPROVED");

            migrationBuilder.DropTable(
                name: "AT_CONFIG");

            migrationBuilder.DropTable(
                name: "AT_DAYOFFYEAR");

            migrationBuilder.DropTable(
                name: "AT_DAYOFFYEAR_CONFIG");

            migrationBuilder.DropTable(
                name: "AT_DECLARE_SENIORITY");

            migrationBuilder.DropTable(
                name: "AT_ENTITLEMENT");

            migrationBuilder.DropTable(
                name: "AT_ENTITLEMENT_EDIT");

            migrationBuilder.DropTable(
                name: "AT_HOLIDAY");

            migrationBuilder.DropTable(
                name: "AT_NOTIFICATION");

            migrationBuilder.DropTable(
                name: "AT_ORG_PERIOD");

            migrationBuilder.DropTable(
                name: "AT_OTHER_LIST");

            migrationBuilder.DropTable(
                name: "AT_OVERTIME");

            migrationBuilder.DropTable(
                name: "AT_OVERTIME_CONFIG");

            migrationBuilder.DropTable(
                name: "AT_PERIOD_STANDARD");

            migrationBuilder.DropTable(
                name: "AT_REGISTER_LEAVE");

            migrationBuilder.DropTable(
                name: "AT_REGISTER_LEAVE_DETAIL");

            migrationBuilder.DropTable(
                name: "AT_SALARY_PERIOD_DTL");

            migrationBuilder.DropTable(
                name: "AT_SETUP_TIME_EMP");

            migrationBuilder.DropTable(
                name: "AT_SIGN_DEFAULT");

            migrationBuilder.DropTable(
                name: "AT_SWIPE_DATA");

            migrationBuilder.DropTable(
                name: "AT_SWIPE_DATA_IMPORT");

            migrationBuilder.DropTable(
                name: "AT_SWIPE_DATA_SYNC");

            migrationBuilder.DropTable(
                name: "AT_SWIPE_DATA_TMP");

            migrationBuilder.DropTable(
                name: "AT_SWIPE_DATA_WRONG");

            migrationBuilder.DropTable(
                name: "AT_SYMBOL");

            migrationBuilder.DropTable(
                name: "AT_TERMINAL");

            migrationBuilder.DropTable(
                name: "AT_TIME_EXPLANATION");

            migrationBuilder.DropTable(
                name: "AT_TIME_LATE_EARLY");

            migrationBuilder.DropTable(
                name: "AT_TIME_TIMESHEET_DAILY");

            migrationBuilder.DropTable(
                name: "AT_TIMESHEET_DAILY");

            migrationBuilder.DropTable(
                name: "AT_TIMESHEET_FORMULA");

            migrationBuilder.DropTable(
                name: "AT_TIMESHEET_LOCK");

            migrationBuilder.DropTable(
                name: "AT_TIMESHEET_MACHINE");

            migrationBuilder.DropTable(
                name: "AT_TIMESHEET_MACHINE_EDIT");

            migrationBuilder.DropTable(
                name: "AT_TIMESHEET_MONTHLY");

            migrationBuilder.DropTable(
                name: "AT_TIMESHEET_MONTHLY_DTL");

            migrationBuilder.DropTable(
                name: "AT_WORKSIGN");

            migrationBuilder.DropTable(
                name: "AT_WORKSIGN_DUTY");

            migrationBuilder.DropTable(
                name: "AT_WORKSIGN_TMP");

            migrationBuilder.DropTable(
                name: "CSS_THEME_VAR");

            migrationBuilder.DropTable(
                name: "DEMO_ATTACHMENT");

            migrationBuilder.DropTable(
                name: "DEMO_HANGFIRE_RECORD");

            migrationBuilder.DropTable(
                name: "HRM_INFOTYPE");

            migrationBuilder.DropTable(
                name: "HRM_OBJECT");

            migrationBuilder.DropTable(
                name: "HU_ALLOWANCE");

            migrationBuilder.DropTable(
                name: "HU_ALLOWANCE_EMP");

            migrationBuilder.DropTable(
                name: "HU_ALLOWANCE_EMP_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_ANSWER_USER");

            migrationBuilder.DropTable(
                name: "HU_BANK");

            migrationBuilder.DropTable(
                name: "HU_BANK_BRANCH");

            migrationBuilder.DropTable(
                name: "HU_CERTIFICATE");

            migrationBuilder.DropTable(
                name: "HU_CERTIFICATE_EDIT");

            migrationBuilder.DropTable(
                name: "HU_CERTIFICATE_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_CLASSIFICATION");

            migrationBuilder.DropTable(
                name: "HU_COMMEND");

            migrationBuilder.DropTable(
                name: "HU_COMMEND_EMP");

            migrationBuilder.DropTable(
                name: "HU_COMMEND_EMPLOYEE");

            migrationBuilder.DropTable(
                name: "HU_COMMEND_EMPLOYEE_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_COMMEND_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_COMPANY");

            migrationBuilder.DropTable(
                name: "HU_CONCURRENTLY");

            migrationBuilder.DropTable(
                name: "HU_CONTRACT_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_DISCIPLINE");

            migrationBuilder.DropTable(
                name: "HU_DISTRICT");

            migrationBuilder.DropTable(
                name: "HU_DYNAMIC_REPORT");

            migrationBuilder.DropTable(
                name: "HU_EMPLOYEE_CV_EDIT");

            migrationBuilder.DropTable(
                name: "HU_EMPLOYEE_CV_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_EMPLOYEE_EDIT");

            migrationBuilder.DropTable(
                name: "HU_EMPLOYEE_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_EMPLOYEE_PAPERS");

            migrationBuilder.DropTable(
                name: "HU_EMPLOYEE_TMP");

            migrationBuilder.DropTable(
                name: "HU_EVALUATE");

            migrationBuilder.DropTable(
                name: "HU_EVALUATE_CONCURRENT_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_EVALUATE_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_EVALUATION_COM");

            migrationBuilder.DropTable(
                name: "HU_EVALUATION_COM_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_FAMILY");

            migrationBuilder.DropTable(
                name: "HU_FAMILY_EDIT");

            migrationBuilder.DropTable(
                name: "HU_FAMILY_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_FILECONTRACT");

            migrationBuilder.DropTable(
                name: "HU_FILECONTRACT_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_FORM_ELEMENT");

            migrationBuilder.DropTable(
                name: "HU_FORM_LIST");

            migrationBuilder.DropTable(
                name: "HU_FROM_ELEMENT");

            migrationBuilder.DropTable(
                name: "HU_JOB_BAND");

            migrationBuilder.DropTable(
                name: "HU_JOB_DESCRIPTION");

            migrationBuilder.DropTable(
                name: "HU_JOB_FUNCTION");

            migrationBuilder.DropTable(
                name: "HU_NATION");

            migrationBuilder.DropTable(
                name: "HU_ORG_LEVEL");

            migrationBuilder.DropTable(
                name: "HU_POS_PAPER");

            migrationBuilder.DropTable(
                name: "HU_POSITION_GROUP");

            migrationBuilder.DropTable(
                name: "HU_PROVINCE");

            migrationBuilder.DropTable(
                name: "HU_QUESTION");

            migrationBuilder.DropTable(
                name: "HU_REPORT");

            migrationBuilder.DropTable(
                name: "HU_SALARY_LEVEL");

            migrationBuilder.DropTable(
                name: "HU_SALARY_RANK");

            migrationBuilder.DropTable(
                name: "HU_SALARY_SCALE");

            migrationBuilder.DropTable(
                name: "HU_SETTING_REMIND");

            migrationBuilder.DropTable(
                name: "HU_TERMINATE");

            migrationBuilder.DropTable(
                name: "HU_TITLE_GROUP");

            migrationBuilder.DropTable(
                name: "HU_WARD");

            migrationBuilder.DropTable(
                name: "HU_WELFARE");

            migrationBuilder.DropTable(
                name: "HU_WELFARE_AUTO");

            migrationBuilder.DropTable(
                name: "HU_WELFARE_CONTRACT");

            migrationBuilder.DropTable(
                name: "HU_WELFARE_MNG");

            migrationBuilder.DropTable(
                name: "HU_WELFARE_MNG_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_WORK_LOCATION");

            migrationBuilder.DropTable(
                name: "HU_WORKING_ALLOW");

            migrationBuilder.DropTable(
                name: "HU_WORKING_ALLOW_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_WORKING_BEFORE");

            migrationBuilder.DropTable(
                name: "HU_WORKING_BEFORE_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_WORKING_HSL_PC_IMPORT");

            migrationBuilder.DropTable(
                name: "HU_WORKING_IMPORT");

            migrationBuilder.DropTable(
                name: "HUV_WAGE_MAX");

            migrationBuilder.DropTable(
                name: "HUV_WORKING_MAX_BYTYPE");

            migrationBuilder.DropTable(
                name: "INS_ARISING");

            migrationBuilder.DropTable(
                name: "INS_CHANGE");

            migrationBuilder.DropTable(
                name: "INS_DECLARATION_NEW");

            migrationBuilder.DropTable(
                name: "INS_GROUP");

            migrationBuilder.DropTable(
                name: "INS_HEALTH_INSURANCE");

            migrationBuilder.DropTable(
                name: "INS_INFORMATION");

            migrationBuilder.DropTable(
                name: "INS_INFORMATION_IMPORT");

            migrationBuilder.DropTable(
                name: "INS_LIST_PROGRAM");

            migrationBuilder.DropTable(
                name: "INS_REGIMES");

            migrationBuilder.DropTable(
                name: "INS_REGIMES_MNG");

            migrationBuilder.DropTable(
                name: "INS_REGION");

            migrationBuilder.DropTable(
                name: "INS_SPECIFIED_OBJECTS");

            migrationBuilder.DropTable(
                name: "INS_TYPE");

            migrationBuilder.DropTable(
                name: "INS_WHEREHEALTH");

            migrationBuilder.DropTable(
                name: "PA_ADVANCE");

            migrationBuilder.DropTable(
                name: "PA_ADVANCE_TMP");

            migrationBuilder.DropTable(
                name: "PA_AUTHORITY_TAX_YEAR");

            migrationBuilder.DropTable(
                name: "PA_CALCULATE_LOCK");

            migrationBuilder.DropTable(
                name: "PA_FORMULA");

            migrationBuilder.DropTable(
                name: "PA_IMPORT_MONTHLY_TAX");

            migrationBuilder.DropTable(
                name: "PA_KPI_FORMULA");

            migrationBuilder.DropTable(
                name: "PA_KPI_LOCK");

            migrationBuilder.DropTable(
                name: "PA_KPI_POSITION");

            migrationBuilder.DropTable(
                name: "PA_KPI_SALARY_DETAIL");

            migrationBuilder.DropTable(
                name: "PA_KPI_SALARY_DETAIL_TMP");

            migrationBuilder.DropTable(
                name: "PA_LIST_FUND_SOURCE");

            migrationBuilder.DropTable(
                name: "PA_LISTFUND");

            migrationBuilder.DropTable(
                name: "PA_LISTSAL");

            migrationBuilder.DropTable(
                name: "PA_LISTSALARIES");

            migrationBuilder.DropTable(
                name: "PA_PAYROLL_FUND");

            migrationBuilder.DropTable(
                name: "PA_PAYROLL_TAX_YEAR");

            migrationBuilder.DropTable(
                name: "PA_PAYROLLSHEET_SUM");

            migrationBuilder.DropTable(
                name: "PA_PAYROLLSHEET_SUM_BACKDATE");

            migrationBuilder.DropTable(
                name: "PA_PAYROLLSHEET_SUM_SUB");

            migrationBuilder.DropTable(
                name: "PA_PERIOD_TAX");

            migrationBuilder.DropTable(
                name: "PA_PHASE_ADVANCE");

            migrationBuilder.DropTable(
                name: "PA_PHASE_ADVANCE_SYMBOL");

            migrationBuilder.DropTable(
                name: "PA_SAL_IMPORT");

            migrationBuilder.DropTable(
                name: "PA_SAL_IMPORT_ADD");

            migrationBuilder.DropTable(
                name: "PA_SAL_IMPORT_BACKDATE");

            migrationBuilder.DropTable(
                name: "PA_SAL_IMPORT_TMP");

            migrationBuilder.DropTable(
                name: "PA_SALARY_PAYCHECK");

            migrationBuilder.DropTable(
                name: "PA_SALARY_STRUCTURE");

            migrationBuilder.DropTable(
                name: "PA_TAX_ANNUAL_IMPORT");

            migrationBuilder.DropTable(
                name: "PaPayrollsheetTaxs");

            migrationBuilder.DropTable(
                name: "PORTAL_CERTIFICATE");

            migrationBuilder.DropTable(
                name: "PORTAL_REGISTER_OFF");

            migrationBuilder.DropTable(
                name: "PORTAL_REGISTER_OFF_DETAIL");

            migrationBuilder.DropTable(
                name: "PORTAL_REQUEST_CHANGE");

            migrationBuilder.DropTable(
                name: "PORTAL_ROUTE");

            migrationBuilder.DropTable(
                name: "PT_BLOG_INTERNAL");

            migrationBuilder.DropTable(
                name: "RC_CANDIDATE_SCANCV");

            migrationBuilder.DropTable(
                name: "RC_EXAMS");

            migrationBuilder.DropTable(
                name: "REPORT_DATA_STAFF_PROFILE");

            migrationBuilder.DropTable(
                name: "SE_APP_PROCESS");

            migrationBuilder.DropTable(
                name: "SE_APP_TEMPLATE");

            migrationBuilder.DropTable(
                name: "SE_APP_TEMPLATE_DTL");

            migrationBuilder.DropTable(
                name: "SE_AUTHORIZE_APPROVE");

            migrationBuilder.DropTable(
                name: "SE_DOCUMENT");

            migrationBuilder.DropTable(
                name: "SE_DOCUMENT_INFO");

            migrationBuilder.DropTable(
                name: "SE_HR_PROCESS");

            migrationBuilder.DropTable(
                name: "SE_HR_PROCESS_DATA_MODEL");

            migrationBuilder.DropTable(
                name: "SE_HR_PROCESS_INSTANCE");

            migrationBuilder.DropTable(
                name: "SE_HR_PROCESS_NODE");

            migrationBuilder.DropTable(
                name: "SE_HR_PROCESS_TYPE");

            migrationBuilder.DropTable(
                name: "SE_LDAP");

            migrationBuilder.DropTable(
                name: "SE_PROCESS");

            migrationBuilder.DropTable(
                name: "SE_PROCESS_APPROVE");

            migrationBuilder.DropTable(
                name: "SE_PROCESS_APPROVE_POS");

            migrationBuilder.DropTable(
                name: "SE_PROCESS_APPROVE_STATUS");

            migrationBuilder.DropTable(
                name: "SE_REMINDER");

            migrationBuilder.DropTable(
                name: "SY_AUDITLOG");

            migrationBuilder.DropTable(
                name: "SYS_ACTION");

            migrationBuilder.DropTable(
                name: "SYS_CONFIGURATION_COMMON");

            migrationBuilder.DropTable(
                name: "SYS_FORM_LIST");

            migrationBuilder.DropTable(
                name: "SYS_FUNCTION_ACTION");

            migrationBuilder.DropTable(
                name: "SYS_FUNCTION_GROUP");

            migrationBuilder.DropTable(
                name: "SYS_FUNCTION_IGNORE");

            migrationBuilder.DropTable(
                name: "SYS_GROUP_FUNCTION_ACTION");

            migrationBuilder.DropTable(
                name: "SYS_GROUP_PERMISSION");

            migrationBuilder.DropTable(
                name: "SYS_KPI");

            migrationBuilder.DropTable(
                name: "SYS_KPI_GROUP");

            migrationBuilder.DropTable(
                name: "SYS_LANGUAGE");

            migrationBuilder.DropTable(
                name: "SYS_MENU");

            migrationBuilder.DropTable(
                name: "SYS_MODULE");

            migrationBuilder.DropTable(
                name: "SYS_MUTATION_LOG");

            migrationBuilder.DropTable(
                name: "SYS_OTHER_LIST_FIX");

            migrationBuilder.DropTable(
                name: "SYS_OTHER_LIST_TYPE");

            migrationBuilder.DropTable(
                name: "SYS_PA_FORMULA");

            migrationBuilder.DropTable(
                name: "SYS_PERMISSION");

            migrationBuilder.DropTable(
                name: "SYS_REFRESH_TOKEN");

            migrationBuilder.DropTable(
                name: "SYS_SALARY_STRUCTURE");

            migrationBuilder.DropTable(
                name: "SYS_SETTING_MAP");

            migrationBuilder.DropTable(
                name: "SYS_TMP_SORT");

            migrationBuilder.DropTable(
                name: "SYS_USER");

            migrationBuilder.DropTable(
                name: "SYS_USER_FUNCTION_ACTION");

            migrationBuilder.DropTable(
                name: "SYS_USER_GROUP_ORG");

            migrationBuilder.DropTable(
                name: "SYS_USER_ORG");

            migrationBuilder.DropTable(
                name: "SYS_USER_PERMISSION");

            migrationBuilder.DropTable(
                name: "SysConfigs");

            migrationBuilder.DropTable(
                name: "THEME_BLOG");

            migrationBuilder.DropTable(
                name: "TMP_HU_CONTRACT");

            migrationBuilder.DropTable(
                name: "TMP_HU_WORKING");

            migrationBuilder.DropTable(
                name: "TMP_INS_CHANGE");

            migrationBuilder.DropTable(
                name: "TR_CENTER");

            migrationBuilder.DropTable(
                name: "TR_COURSE");

            migrationBuilder.DropTable(
                name: "TR_PLAN");

            migrationBuilder.DropTable(
                name: "UniqueIndexes");

            migrationBuilder.DropTable(
                name: "AD_ROOM");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AT_REGISTER_OFF");

            migrationBuilder.DropTable(
                name: "AT_SHIFT");

            migrationBuilder.DropTable(
                name: "CSS_THEME");

            migrationBuilder.DropTable(
                name: "CSS_VAR");

            migrationBuilder.DropTable(
                name: "HU_ANSWER");

            migrationBuilder.DropTable(
                name: "HU_CONTRACT");

            migrationBuilder.DropTable(
                name: "HU_JOB");

            migrationBuilder.DropTable(
                name: "SYS_OTHER_LIST");

            migrationBuilder.DropTable(
                name: "HU_ORGANIZATION");

            migrationBuilder.DropTable(
                name: "HU_POSITION");

            migrationBuilder.DropTable(
                name: "PA_KPI_TARGET");

            migrationBuilder.DropTable(
                name: "AT_SALARY_PERIOD");

            migrationBuilder.DropTable(
                name: "HU_SALARY_TYPE");

            migrationBuilder.DropTable(
                name: "PA_ELEMENT");

            migrationBuilder.DropTable(
                name: "SYS_FUNCTION");

            migrationBuilder.DropTable(
                name: "SYS_GROUP");

            migrationBuilder.DropTable(
                name: "SYS_PA_ELEMENT");

            migrationBuilder.DropTable(
                name: "SYS_SALARY_TYPE");

            migrationBuilder.DropTable(
                name: "AT_TIME_TYPE");

            migrationBuilder.DropTable(
                name: "HU_CONTRACT_TYPE");

            migrationBuilder.DropTable(
                name: "HU_EMPLOYEE");

            migrationBuilder.DropTable(
                name: "HU_WORKING");

            migrationBuilder.DropTable(
                name: "PA_KPI_GROUP");

            migrationBuilder.DropTable(
                name: "PA_ELEMENT_GROUP");

            migrationBuilder.DropTable(
                name: "SYS_CONTRACT_TYPE");

            migrationBuilder.DropTable(
                name: "HU_EMPLOYEE_CV");
        }
    }
}
