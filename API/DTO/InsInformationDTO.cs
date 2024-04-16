using API.Main;

namespace API.DTO
{
    public class InsInformationDTO:BaseDTO
    {
        public long? EmployeeId { get; set; }
        public int? JobOrderNum { get; set; }
        public long? ProfileId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PositionName { get; set; }
        public string? OrgName { get; set; }
        public long? OrgId { get; set; }
        public string? IdNo { get; set; }
        public DateTime? IdDate { get; set; }
        public string? IdDateStr { get; set; }
        public string? AddressIdentity { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BirthDateStr { get; set; }
        public string? BirthPlace { get; set; }
        public string? Contact { get; set; }
        public string? ListInsuranceStr { get; set; }

        public int? SeniorityInsurance { get; set; }
        public int? SeniorityInsuranceInCompany { get; set; }
        public string? SeniorityInsuranceString { get; set; }
        public string? SeniorityInsuranceInCompanyString { get; set; }
        public string? Company { get; set; }
        public double? SalaryBhxhYt { get; set; }
        public string? SalaryBhxhYtStr { get; set; }
        public double? SalaryBhTn { get; set; }
        public double? SalaryRegionMin { get; set; }
        public double? SalaryNew { get; set; }

        //Bhxh
        public DateTime? BhxhFromDate { get; set; }
        public DateTime? BhxhToDate { get; set; }
        public string? BhxhNo { get; set; }
        public long? BhxhStatusId { get; set; }
        public string? BhxhStatusString { get; set; }
        public DateTime? BhxhSuppliedDate { get; set; }
        public string? BhxhSuppliedDateStr { get; set; }
        public DateTime? BhxhReimbursementDate { get; set; }
        public string? BhxhReimbursementDateStr { get; set; }
        public DateTime? BhxhGrantDate { get; set; }
        public string? BhxhGrantDateStr { get; set; }
        public string? BhxhDeliverer { get; set; }
        public string? BhxhStorageNumber { get; set; }
        public string? BhxhReceiver { get; set; }
        public string? BhxhNote { get; set; }

        //Bhyt
        public string? BhytNo { get; set; }
        public DateTime? BhytFromDate { get; set; }
        public DateTime? BhytToDate { get; set; }
        public DateTime? BhytEffectDate { get; set; }
        public string? BhytEffectDateStr { get; set; }
        public DateTime? BhytExpireDate { get; set; }
        public string? BhytExpireDateStr { get; set; }
        public long? BhytStatusId { get; set; }
        public string? BhytStatusString { get; set; }
        public long? BhytWherehealthId { get; set; }
        public string? BhytWherehealthString { get; set; }
        public DateTime? BhytReceivedDate { get; set; }
        public string? BhytReceivedDateStr { get; set; }
        public string? BhytReceiver { get; set; }
        public DateTime? BhytReimbursementDate { get; set; }
        public string? BhytReimbursementDateStr { get; set; }

        //Bhtn
        public DateTime? BhtnFromDate { get; set; }
        public DateTime? BhtnToDate { get; set; }

        //Bhtnld-Bnn
        public DateTime? BhtnldBnnFromDate { get; set; }
        public DateTime? BhtnldBnnToDate { get; set; }

        public bool? IsBhxh { get; set; }
        public bool? IsBhyt { get; set; }
        public bool? IsBhtnldBnn { get; set; }
        public bool? IsBhtn { get; set; }

        public List<int>? LstCheckInsItems { get; set; }

        public string? BhxhFromDateString { get; set; }
        public string? BhxhToDateString { get; set; }
        public string? BhytFromDateString { get; set; }
        public string? BhytToDateString { get; set; }
        public string? BhtnFromDateString { get; set; }
        public string? BhtnToDateString { get; set; }
        public string? BhtnldBnnFromDateString { get; set; }
        public string? BhtnldBnnToDateString { get; set; }
        
        public long? IdPos { get; set; }

    }

}
