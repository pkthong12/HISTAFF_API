using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class PositionViewDTO : Pagings
    {
        public long? Id { get; set; }
        public long? GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? Name { get; set; }
        public string? NameOnProfileEmployee { get; set; }
        public string? InsurenceArea { get; set; }
        public long? InsurenceAreaId { get; set; }
        public string? NameEn { get; set; }
        public string? Code { get; set; }
        public bool? IsActive { get; set; }
        public string? JobDesc { get; set; }
        public string? Note { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public long? JobId { get; set; }
        public string? JobName { get; set; }
        public int? Lm { get; set; }
        public string? LmName { get; set; }
        public long? LmNameId { get; set; }
        public string? LmJobName { get; set; }
        public long? LmJobNameId { get; set; }
        public bool? isowner { get; set; }
        public int? csm { get; set; }
        public string? CsmName { get; set; }
        public string? CsmJobName { get; set; }
        public bool? isnonphysical { get; set; }
        public bool? isMaster { get; set; }
        public long? Master { get; set; }
        public string? MasterName { get; set; }
        public string? mastercode { get; set; }
        public bool? isConcurrently { get; set; }
        public int? concurrent { get; set; }
        public bool? isplan { get; set; }
        public bool? isInterim { get; set; }
        public long? Interim { get; set; }
        public string? InterimName { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string? typeactivities { get; set; }
        public string? filename { get; set; }
        public string? uploadfile { get; set; }
        public int? workingtime { get; set; }
        //public PositionDesriptionInputDTO? _positionDesc { get; set; }
        public string? remark { get; set; }
        public long? hiringStatus { get; set; }
        public int? flag { get; set; }
        public int? both { get; set; }
        public string? color { get; set; }
        public int? orgIdSearch { get; set; }
        public int? orgId2Search { get; set; }
        public string? textboxSearch { get; set; }
        public string? textbox2Search { get; set; }
        public bool? isTDV { get; set; }
        public bool? isNotot { get; set; }
        public string? OrgCode { get; set; }
        public string? EmployeeCode { get; set; }
        public string? ConcurrentName { get; set; }
        public long? OrgConcurrentId { get; set; }
        public string? OrgConcurrentName { get; set; }
        public string? Company { get; set; }
        public string? WorkAddress { get; set; }
    }
    public class PositionViewNoPagingDTO
    {
        public long? Id { get; set; }
        public long? GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? Name { get; set; }
        public string? NameOnProfileEmployee { get; set; }
        public string? InsurenceArea { get; set; }
        public long? InsurenceAreaId { get; set; }
        public string? NameEn { get; set; }
        public string? Code { get; set; }
        public bool? IsActive { get; set; }
        public string? JobDesc { get; set; }
        public string? Note { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? OrgId { get; set; }
        public string? OrgName { get; set; }
        public long? JobId { get; set; }
        public string? JobName { get; set; }
        public int? Lm { get; set; }
        public string? LmName { get; set; }
        public string? EmpLmName { get; set; }
        public long? LmNameId { get; set; }
        public string? LmJobName { get; set; }
        public long? LmJobNameId { get; set; }
        public bool? isowner { get; set; }
        public int? csm { get; set; }
        public string? CsmName { get; set; }
        public string? CsmJobName { get; set; }
        public bool? isnonphysical { get; set; }
        public bool? isMaster { get; set; }
        public long? Master { get; set; }
        public string? MasterName { get; set; }
        public string? mastercode { get; set; }
        public bool? isConcurrently { get; set; }
        public int? concurrent { get; set; }
        public bool? isplan { get; set; }
        public bool? isInterim { get; set; }
        public long? Interim { get; set; }
        public string? InterimName { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string? typeactivities { get; set; }
        public string? filename { get; set; }
        public string? uploadfile { get; set; }
        public int? workingtime { get; set; }
        //public PositionDesriptionInputDTO? _positionDesc { get; set; }
        public string? remark { get; set; }
        public long? hiringStatus { get; set; }
        public int? flag { get; set; }
        public int? both { get; set; }
        public string? color { get; set; }
        public int? orgIdSearch { get; set; }
        public int? orgId2Search { get; set; }
        public string? textboxSearch { get; set; }
        public string? textbox2Search { get; set; }
        public bool? isTDV { get; set; }
        public bool? isNotot { get; set; }
        public string? OrgCode { get; set; }
        public string? ComCode { get; set; }
        public string? EmployeeCode { get; set; }
        public string? ConcurrentName { get; set; }
        public long? OrgConcurrentId { get; set; }
        public string? OrgConcurrentName { get; set; }
        public string? Company { get; set; }
        public string? WorkAddress { get; set; }
        public string? Active { get; set; }
        public long? StatusChair { get; set; }
        public long? ConcurrentStatus { get; set; }

    }
    public class PositionOutputDTO
    {
        public long Id { get; set; }
        public string? jobDesc { get; set; }
        public long? GroupId { get; set; }
        public string? Name { get; set; }
        public string? NameEn { get; set; }
        public string? Code { get; set; }
        public bool? IsActive { get; set; }
        public string? Note { get; set; }
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? OrgId { get; set; }
        public string? orgname { get; set; }
        public long? jobid { get; set; }
        public string? jobname { get; set; }
        public long? lm { get; set; }
        public string? lmname { get; set; }
        public string? lmposition { get; set; }
        public bool? isowner { get; set; }
        public long? csm { get; set; }
        public string? csmname { get; set; }
        public string? csmposition { get; set; }
        public bool? isnonphysical { get; set; }
        public long? master { get; set; }
        public string? MasterName { get; set; }
        public long? concurrent { get; set; }
        public bool? isplan { get; set; }
        public long? interim { get; set; }
        public string? interimname { get; set; }
        public DateTime? effectivedate { get; set; }
        public string? typeactivities { get; set; }
        public string? filename { get; set; }
        public string? uploadfile { get; set; }
        public long? workingtime { get; set; }
        //public PositionDesriptionOutputDTO positionDesc { get; set; }
        public string? remark { get; set; }
        public long? hiringStatus { get; set; }
        public int? flag { get; set; }
        public int? both { get; set; }
        public string? color { get; set; }
        public long? orgIdSearch { get; set; }
        public long? orgId2Search { get; set; }
        public string? textboxSearch { get; set; }
        public string? textbox2Search { get; set; }
        public bool? isMaster { get; set; }
        public string? mastercode { get; set; }
        public bool? isConcurrently { get; set; }
        public bool? isInterim { get; set; }
        public bool? isTDV { get; set; }
        public bool? isNotot { get; set; }
        public decimal? OrderNum { get; set; }

    }

    public class PositionInputDTO
    {
        public long? Id { get; set; }
        public long? GroupId { get; set; }
        public string? code { get; set; }
        public string? name { get; set; }
        public string? NameEn { get; set; }
        public string? note { get; set; }
        public string? jobDesc { get; set; }
        public long? TenantID { get; set; }
        public long? isActive { get; set; }
        public string? createBy { get; set; }
        public string? updatedBy { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updatedDate { get; set; }
        public long? OrgId { get; set; }
        public long? jobid { get; set; }
        public int? lm { get; set; }
        public bool? isowner { get; set; }
        public long? csm { get; set; }
        public bool? isnonphysical { get; set; }
        public long? master { get; set; }
        public string? masterName { get; set; }
        public int? concurrent { get; set; }
        public bool? isplan { get; set; }
        public int? interim { get; set; }
        public DateTime? effectivedate { get; set; }
        public string? typeactivities { get; set; }
        public string? filename { get; set; }
        public string? uploadfile { get; set; }
        public int? workingtime { get; set; }
        public bool? isPlanLeft { get; set; }
        //public PositionDesriptionInputDTO _positionDesc { get; set; }
        public string? remark { get; set; }
        public int? hiringStatus { get; set; }
        public int? flag { get; set; }
        public int? both { get; set; }
        public string? color { get; set; }
        public int? orgIdSearch { get; set; }
        public int? orgId2Search { get; set; }
        public string? textboxSearch { get; set; }
        public string? textbox2Search { get; set; }
        public int? workLocation { get; set; }
        public bool? isMaster { get; set; }
        public string? mastercode { get; set; }
        public bool? isConcurrently { get; set; }
        public bool? isInterim { get; set; }
        public bool? isTDV { get; set; }
        public bool? isNotot { get; set; }
        public List<long?>? Ids { get; set; }
        public bool? ValueToBind { get; set; }
        public bool? ConfirmChangeTdv { get; set; }



        //Added by Datnv 
        public List<long>? ListId { get; set; }
        public long? OrgIdTransfer { get; set; }
        public string? UserId { get; set; }
        public int?  Amount { get; set; }



    }

}
