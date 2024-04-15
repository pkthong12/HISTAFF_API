using API.Main;

namespace API.DTO
{
    public class PaListsalariesDTO : BaseDTO 
    {
        public long? CodeSal { get; set; }//ma danh muc luong
        public string? CodeSalName { get; set; }
        public string? Name { get; set; }//ten ket cau
        public long? DataType { get; set; }//kieu dl
        public string? DataTypeName { get; set; }
        public long? ObjSalId{get;set;}//doi tuong luong
        public string? ObjSalName { get; set; }//NHOM CT
        public long? GroupType { get; set; }//nhom ky hieu
        public string? GroupTypeName { get; set; }
        public int? ColIndex { get; set; }//so thu tu
        public bool? IsActive { get; set; }//trang thai
        public string? Status { get; set; }
        public bool? IsVisible { get; set; }//hien thi trong sal
        public bool? IsImport { get; set; }//dl import
        public bool? IsQlTypeTn { get; set; }//QL
        public bool? IsFormula { get; set; }//cong thuc theo bien dong
        public bool? IsSumFormula { get; set; }//tong theo bien dong
        public bool? IsPayback { get; set; }//payback
        public string? Note { get; set; }
        public DateTime? EffectiveDate { get; set; }//ngay hieu luc
        public DateTime? ExpireDate { get; set; }//ngay het han
        public string? CreatedLog {  get; set; }
        public string? UpdatedLog {  get; set; }
    }
}
