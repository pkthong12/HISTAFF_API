using Common.Paging;

namespace PayrollDAL.ViewModels
{
    public class PaycheckDTO : Pagings
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int SalaryId { get; set; }
        public string SalaryTypeName { get; set; }
        public string ElementName { get; set; }
        public bool? IsVisible { get; set; }
        public int? Orders { get; set; }
    }

    public class PaycheckInputDTO
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public int? SalaryTypeId { get; set; }
        public int? ElementId { get; set; }
        public bool? IsVisible { get; set; }
        public int? Orders { get; set; }
    }

    public class PaycheckInputListDTO
    {
        public int SalaryTypeId { get; set; }
        public List<int> ElementId { get; set; }
    }

    //public class SalaryStructureQuickDTO
    //{
    //    public long? Id { get; set; }
    //    public bool? IsVisible { get; set; }
    //    public bool? IsCalculate { get; set; }
    //    public bool? IsImport { get; set; }
    //}

}
