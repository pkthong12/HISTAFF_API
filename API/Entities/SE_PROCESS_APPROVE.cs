using API.DTO;
using API.Main;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;
[Table("SE_PROCESS_APPROVE")]
public class SE_PROCESS_APPROVE : BASE_ENTITY
{
    //public long? ORG_ID { get; set; }//SDTC
    public string? APPROVAL_LEVEL_NAME { get; set; }//Ten cap phe duyet
    public int? LEVEL_ORDER_ID { get; set; }//Thu tu cap
    public bool? APPROVAL_POSITION { get; set; }//Bo qua neu vi tri phe duyet trong
    public bool? SAME_APPROVER { get; set; }//Bo qua neu trung nguoi phe duyet
    public long? PROCESS_ID { get; set; }//Quy trinh
    public bool? DIRECT_MANAGER { get; set; }//Quan li truc tiep
    public bool? MANAGER_AFFILIATED_DEPARTMENTS { get; set; }//Quan ly phong ban truc thuoc
    public bool? MANAGER_SUPERIOR_DEPARTMENTS { get; set; }//Quan ly phong ban cap tren
    public bool? IS_DIRECT_MNG_OF_DIRECT_MNG { get; set; }

    //public long? POS_ID { get; set; }//Chôn truc danh phe duyet
    //public long? APPROVAL_POS_ID { get; set; }//Chon nhom chuc danh phe duyet

    public string? APPROVE { get; set; }//Phe duyet
    public string? REFUSE { get; set; }//Tu choi
    public string? CHOOSE_AN_APPROVER { get; set; }//Lua chon nguoi phe duyet

    public string? UPDATED_LOG { get; set; }
    public string? CREATED_LOG { get; set; }

}

