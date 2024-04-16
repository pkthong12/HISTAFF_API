using Common.Paging;
using CORE.Services.File;

namespace ProfileDAL.ViewModels
{
    public class WorkingDTO: Pagings
    {
        public long? Id { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long? WorkStatusId { get; set; }
        public string? PositionName { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public DateTime? EffectDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? DecisionNo { get; set; } // Số Quyết định
        public long? TypeId { get; set; } // Loại Quyết định
        public string? TypeName { get; set; } // Loại Quyết định        
        public long? StatusId { get; set; } // Trạng thái
        public string? StatusName { get; set; }
        public string? SignerName { get; set; } // Tên người ký
        public string? SignerCode { get; set; } // Tên người ký
        public string? SignerPosition { get; set; } // Chức danh người ký
        public string? Note { get; set; }
        public string? SalaryType { get; set; }
        public DateTime? SignDate { get; set; } // Ngày ký
        public decimal? SalBasic { get; set; } // Lương cơ bản
        public decimal? SalTotal { get; set; } // Tổng lương
        public decimal? SalPercent { get; set; } // Tỷ lệ hưởng lương
         public string? SalaryScaleName { get; set; } // Thang lương
        public string? SalaryRankName { get; set; } // Ngạch lương
        public string? SalaryLevelName { get; set; } // Bậc lương
        public List<WorkingAllowInputDTO>? Allowances { get; set; }
        public List<decimal?>? OrgIds { get; set; }
        public string? EmployeeObjName { get; set; }
        public string? LaborObjName { get; set; }

        public decimal? ShortTempSalary { get; set; }
        public string? TaxtableName { get; set; }
        public string? regionName { get; set; }
        public string? salaryTypeName { get; set; }
        public decimal? Coefficient { get; set; }
        public string? salaryScaleDcvName { get; set; }
        public string? salaryRankDcvName { get; set; }
        public string? salaryLevelDcvName { get; set; }

        public decimal? CoefficientDcv { get; set; }
        public DateTime? EffectUpsalDate { get; set; }
        public string? ReasonUpsal { get; set; }
    }

    public class WorkingInputDTO
    {
        public long? Id { get; set; }
        public long? Tenant_Id { get; set; }
        //[Required(ErrorMessage = "{0}_Required")]
        public long? EmployeeId { get; set; }
        //[Required(ErrorMessage = "{0}_Required")]
        public long? PositionId { get; set; }
        //[Required(ErrorMessage = "{0}_Required")]
        public long? OrgId { get; set; }
        //[Required(ErrorMessage = "{0}_Required")]
        public long? TypeId { get; set; } // Loại Quyết định
        public string? TypeCode { get; set; }
        //[Required(ErrorMessage = "{0}_Required")]
        public string? DecisionNo { get; set; }
        //[Required(ErrorMessage = "{0}_Required")]
        public DateTime? EffectDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        //[Required(ErrorMessage = "{0}_Required")]
        public long? StatusId { get; set; } // Trạng thái
        public long? SalaryTypeId { get; set; } // Bảng lương
        public long? SalaryScaleId { get; set; } // Thang lương
        public long? SalaryRankId { get; set; } // Ngạch lương
        public long? SalaryLevelId { get; set; } // Bậc lương
        public string? SalaryTypeName { get; set; } // Bảng lương
        public string? SalaryScaleName { get; set; } // Thang lương
        public string? SalaryRankName { get; set; } // Ngạch lương
        public string? SalaryLevelName { get; set; } // Bậc lương
        public decimal? Coefficient { get; set; }
        public decimal? SalBasic { get; set; } // Lương cơ bản
        public decimal? SalTotal { get; set; } // Tổng lương
        public decimal? SalPercent { get; set; } // Tỷ lệ hưởng lương
        public long? SignId { get; set; } // Người ký
        public long? ReligionId { get; set; } // Vùng
        public string? SignerName { get; set; } // Tên người ký
        public string? SignerPosition { get; set; } // Chức danh người ký
        public DateTime? SignDate { get; set; } // Ngày ký
        public string? orgParentName { get; set; }
        public List<WorkingAllowInputDTO>? Allowances { get; set; }
        public int? isWage { get; set; }
        public DateTime? expireUpsalDate { get; set; }
        public int? isBHXH { get; set; }
        public int? isBHYT { get; set; }
        public int? isBHTNLDBNN { get; set; }
        public int? isBHTN { get; set; }
        public decimal? coefficientDcv { get; set; }
        public decimal? shortTempSalary { get; set; }
        public long? taxtableId { get; set; }
        public long? SalaryScaleDcvId { get; set; } // Thang lương DCV
        public long? SalaryRankDcvId { get; set; } // Ngạch lương DCV
        public long? SalaryLevelDcvId { get; set; } // Bậc lương DCV
        public List<decimal>? lstCheckIns { get; set; }
        public string? Attachment { get; set; }
        public AttachmentDTO? AttachmentBuffer { get; set; }
        public string? Note { get; set; }
        public long? EmployeeObjId { get; set; }
        public long? LaborObjId { get; set; }
        public long? WageId { get; set; }
        public bool? IsResponsible { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? OrgName { get; set; }
        public string? WorkPlaceName { get; set; }
        public string? EmployeeObjName { get; set; }
        public string? LaborObjName { get; set; }
        public string? TypeName { get; set; }
        public string? PositionName { get; set; }
        public DateTime? BaseDate { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime? CreatedDateDecision { get; set; }
        public decimal? ConfirmSwapMasterInterim { get; set; }

        // Update Upsal
        public List<long>? ids { get; set; }
        public string? dateCal { get; set; }
        public string? reasonUpsal { get; set; }

        public bool? ValueToBind { get; set; }


    }

    public class ImportDtlParam
    {
        public string Code { get; set; }
        public string FullName { get; set; }
        public string TypeName { get; set; }
        public string DecisionNo { get; set; }
        public string EffectDate { get; set; }
        public string OrgId { get; set; }
        public string PosName { get; set; }
        public string SalaryTypeName { get; set; }
        public string SalaryLevelId { get; set; }
       // public string Coefficient { get; set; }
        public string SalaryBasic { get; set; }
        public string SalaryTotal { get; set; }
        public string SalaryPercent { get; set; }
        public string StatusName { get; set; }
        public string SignDate { get; set; }
        public string SignName { get; set; }
        public string SignPosition { get; set; }
    }

    public class ImportParam
    {
        public List<ImportDtlParam> Data { get; set; }
    }
}
