using API.DTO;
using Common.Paging;
using CORE.Services.File;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class ContractAppendixDTO : Pagings
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? WorkStatusId { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? ContractAppendixNo { get; set; } // Số phụ lục HĐ
        public string? ContractNo { get; set; } // Số HĐ
        public int? ContractTypeId { get; set; } // Loại HĐ
        public string? ContractTypeName { get; set; } // Tên loại HĐ    
        public long? StatusId { get; set; } // Trạng thái
        public string? StatusName { get; set; }
        public string? SignerName { get; set; } // Tên người ký
        public string? SignerPosition { get; set; } // Chức danh người ký
        //public string Note { get; set; } 
        public DateTime? SignDate { get; set; }
        public long? EmpStatus { get; set; }
        public int? JobOrderNum { get; set; }
        public string? InformationEdit { get; set; }

    }

    public class ContractAppendixInputDTO
    {
        public long? Id { get; set; }
        //[Required(ErrorMessage ="{0}_Required")]
        public long EmployeeId { get; set; }
        //[Required(ErrorMessage = "{0}_Required")]
        public string? ContractNo { get; set; } // Số HĐ
        public long? IdContract { get; set; }
        //[Required(ErrorMessage = "{0}_Required")]
        public long? AppendTypeid { get; set; } // Loại Quyết định
        //[Required(ErrorMessage = "{0}_Required")]
        public DateTime StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public long? StatusId { get; set; } // Trạng thái
        //[Required(ErrorMessage = "{0}_Required")]
        public long? WorkingId { get; set; } // Bảng lương
        public long? SignId { get; set; } // Người ký
        public string? SignerName { get; set; } // Tên người ký
        public string? SignerPosition { get; set; } // Chức danh người 
        public DateTime? SignDate { get; set; } // Ngày ký
        public decimal SalBasic { get; set; }
        public decimal SalPercent { get; set; }
        public string? Note { get; set; }
        public AttachmentDTO? UploadFileBuffer { get; set; }
        public long? OrgId { get; set; }
        public int? PositionId { get; set; }
        public List<long>? ids { get; set; }
        public bool? ValueToBind { get; set; }


        // thuộc tính
        // để làm chức năng upload file
        public AttachmentDTO? AttachmentBuffer { get; set; }
        public string? UploadFile { get; set; }
        public string? InformationEdit { get; set; }
    }

    public class HuFilecontractImportDTO
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? WorkStatusId { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public string? PositionName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? ContractAppendixNo { get; set; } // Số phụ lục HĐ
        public string? ContractNo { get; set; } // Số HĐ
        public long? ContractTypeId { get; set; } // Loại HĐ
        public string? ContractTypeName { get; set; } // Tên loại HĐ    
        public long? StatusId { get; set; } // Trạng thái
        public string? StatusName { get; set; }
        public string? SignerName { get; set; } // Tên người ký
        public string? SignerPosition { get; set; } // Chức danh người ký
        public string? Note { get; set; }
        //public string Note { get; set; } 
        public DateTime? SignDate { get; set; }

        public long? EmpStatus { get; set; }
        public string? XlsxUserId { get; set; }
        public string? XlsxExCode { get; set; }
        public DateTime? XlsxInsertOn { get; set; }
        public long? XlsxSession { get; set; }
        public string? XlsxFileName { get; set; }
        public int? XlsxRow { get; set; }
    }
}
