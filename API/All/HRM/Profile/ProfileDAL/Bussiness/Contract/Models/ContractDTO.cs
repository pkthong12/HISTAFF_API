using Common.Paging;
using CORE.Services.File;
using System.ComponentModel.DataAnnotations;

namespace ProfileDAL.ViewModels
{
    public class ContractDTO : Pagings
    {
        public Int64 Id { get; set; }
        public Int64? EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long? WorkStatusId { get; set; }
        public long? OrgId { get; set; }
        public string OrgName { get; set; }
        public string? PositionName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string ContractNo { get; set; } // Số HĐ
        public int? ContractTypeId { get; set; } // Loại HĐ
        public string? ContractTypeName { get; set; } // Tên loại HĐ    
        public long? StatusId { get; set; } // Trạng thái
        public string? StatusName { get; set; }
        public string? SignerName { get; set; } // Tên người ký
        public string? SignerPosition { get; set; } // Chức danh người ký
        //public string Note { get; set; } 
        public DateTime? SignDate { get; set; }
        public string ContractTypeCode { get; set; }
        public bool? IsReceive { get; set; }
        //public string? UploadFile { get; set; }

        public DateTime? LiquidationDate { get; set; }
        public string? LiquidationReason { get; set; }
        public int? JobOrderNum { get; set; }

        //public string CreateBy { get; set; }
        //public string UpdatedBy { get; set; }
        //public DateTime? CreateDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
    }

    public class ContractInputDTO
    {
        public long? Id { get; set; }
        //[Required(ErrorMessage ="{0}_Required")]
        public long EmployeeId { get; set; }
        //[Required(ErrorMessage = "{0}_Required")]
        public string? ContractNo { get; set; } // Số HĐ
        //[Required(ErrorMessage = "{0}_Required")]
        public long? ContractTypeId { get; set; } // Loại Quyết định
        //[Required(ErrorMessage = "{0}_Required")]
        public DateTime? StartDate { get; set; }
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
        public string? UploadFile { get; set; }
        public List<long>? ids { get; set; }
        public bool? ValueToBind { get; set; }
        public bool? IsReceive { get; set; }


        // Liquidation Contract
        public DateTime? DateLiquidation { get; set; }
        public string? ReasonLiquidation { get; set; }
    }
    public class ImportCTractDtlParam
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public string OrgId { get; set; }
        public string PosName { get; set; }
        public string TypeName { get; set; }
        public string ContractNo { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string SalaryBasic { get; set; }
        public string SalaryTotal { get; set; }
        public string SalaryPercent { get; set; }
        public string StatusName { get; set; }
        public string SignDate { get; set; }
        public string SignName { get; set; }
        public string SignPosition { get; set; }
    }

    public class ImportCTractParam
    {
        public List<ImportCTractDtlParam> Data { get; set; }
    }
}
