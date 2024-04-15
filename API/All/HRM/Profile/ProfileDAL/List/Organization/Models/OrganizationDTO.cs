using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class OrganizationDTO
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string infor1 { get; set; }
        public string ParentName { get; set; }
        public long? ParentId { get; set; }
        public long? MngId { get; set; }
        public string MngName { get; set; }
        public DateTime? FoundationDate { get; set; }
        public DateTime? DissolveDate { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string BusinessNumber { get; set; }
        public DateTime? BusinessDate { get; set; }
        public string TaxCode { get; set; }
        public string Note { get; set; }
        public string PosName { get; set; }
        public string Avatar { get; set; }
        public bool? expanded { get; set; }
        public List<OrganizationDTO> Child { get; set; }
        public string orgLvlName { get; set; }
        public string shortName { get; set; }
        public int? groupProject { get; set; }
    }
   
    public class OrganizationInputDTO :Pagings
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string ParentName { get; set; }
        public long? MngId { get; set; }
        public DateTime? FoundationDate { get; set; }
        public DateTime? DissolveDate { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string BusinessNumber { get; set; }
        public DateTime? BusinessDate { get; set; }
        public string TaxCode { get; set; }
        public string Note { get; set; }
    }

}
