using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("HU_CONTRACT_IMPORT")]
    [PrimaryKey(nameof(XLSX_USER_ID), nameof(XLSX_EX_CODE), nameof(XLSX_SESSION), nameof(XLSX_ROW))]
    public class HU_CONTRACT_IMPORT : BASE_IMPORT
    {
        public long? ID { get; set; }
        public long? PROFILE_ID { get; set; }
        public long? EMPLOYEE_ID { get; set; }

        public string? EMPLOYEE_CODE { get; set; } 
        public string? EMPLOYEE_NAME { get; set; } 
        public string? CONTRACT_NO { get; set; } = null!;

        public long? CONTRACT_TYPE_ID { get; set; }

        public DateTime? START_DATE { get; set; }

        public DateTime? EXPIRE_DATE { get; set; }
        public long? ORG_ID { get; set; }
        public long? POSITION_ID { get; set; }
        public long? SIGN_PROFILE_ID { get; set; }
        public long? SIGN_ID { get; set; }

        public string? SIGNER_NAME { get; set; }

        public long? SIGNER_ORG_ID { get; set; }
        public string? SIGNER_POSITION { get; set; }

        public DateTime? SIGN_DATE { get; set; }

        public decimal? SAL_BASIC { get; set; }

        public decimal? SAL_PERCENT { get; set; }

        public string? NOTE { get; set; }

        public long? STATUS_ID { get; set; }

        public long? WORKING_ID { get; set; }

        public string? CREATED_BY { get; set; }

        public string? UPDATED_BY { get; set; }

        public DateTime? CREATED_DATE { get; set; }

        public DateTime? UPDATED_DATE { get; set; }
        public string? UPLOAD_FILE { get; set; }
        public bool? IS_RECEIVE { get; set; }

    }
}
