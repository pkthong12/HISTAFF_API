using Common.Paging;

namespace PayrollDAL.ViewModels
{
    public class SalaryStructureDTO : Pagings
    {
        public long Id { get; set; }
        public string ColName{ get; set; }
        public string Name { get; set; }
        public int SalaryId { get; set; }
        public string SalaryTypeName { get; set; }
        public string GroupName { get; set; }
        public string ElementName { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsCalculate { get; set; }
        public bool? IsImport { get; set; }
        public bool? IsSum { get; set; }
        public bool? IsChange { get; set; }
        public int? Orders { get; set; }
    }

    public class SalaryStructureInputDTO
    {
        public long? Id { get; set; }
        public string ColName { get; set; }
        public string Name { get; set; }
        public int? SalaryTypeId { get; set; }
        public int? ElementId { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsCalculate { get; set; }
        public bool? IsImport { get; set; }
        public bool? IsSum { get; set; }
        public bool? IsChange { get; set; }
        public int? Orders { get; set; }
    }

    public class SalaryStructureQuickDTO
    {
        public long? Id { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsCalculate { get; set; }
        public bool? IsImport { get; set; }
        public bool? IsSum { get; set; }
        public bool? IsChange { get; set; }
    }

}
