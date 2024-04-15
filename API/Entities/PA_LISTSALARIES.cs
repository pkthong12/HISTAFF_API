using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("PA_LISTSALARIES")]
    public class PA_LISTSALARIES : BASE_ENTITY
    {
        public long? CODE_SAL { get; set; }
        public string? NAME { get; set; }//ten ket cau
        public long? DATA_TYPE { get; set; }//kieu dl
        public long? OBJ_SAL_ID { get; set; }//doi tuong luong
        public long? GROUP_TYPE { get; set; }//nhom ky hieu
        public int? COL_INDEX { get; set; }//so thu tu
        public bool? IS_ACTIVE { get; set; }//trang thai
        public bool? IS_VISIBLE { get; set; }//hien thi trong sal
        public bool? IS_IMPORT { get; set; }//dl import
        public bool? IS_QL_TYPE_TN { get; set; }//QL
        public bool? IS_FORMULA { get; set; }//cong thuc theo bien dong
        public bool? IS_SUM_FORMULA { get; set; }//tong theo bien dong
        public bool? IS_PAYBACK { get; set; }//payback
        public string? NOTE { get; set; }
        public DateTime? EFFECTIVE_DATE { get; set; }//ngay hieu luc
        public DateTime? EXPIRE_DATE { get; set; }//ngay het han
        public string? CREATED_LOG { get; set; }
        public string? UPDATED_LOG { get; set; }
    }
}
