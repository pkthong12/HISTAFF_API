using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class InsuranceTypeDTO : Pagings
    {
        public long? Id { get; set; }
        public long? TypeId { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
   
    public class InsuranceTypeInputDTO
    {
        public long? Id { get; set; }
        public long TypeId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }

    }

}
