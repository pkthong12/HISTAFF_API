using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class SalaryStructureSysDTO : Pagings
    {
        public long Id { get; set; }
        public string ColName{ get; set; }
        public string Name { get; set; }
        public string SalaryTypeName { get; set; }
        public string GroupName { get; set; }
        public string ElementName { get; set; }
        public long ElementId { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsCalculate { get; set; }
        public bool? IsImport { get; set; }
        public int? Orders { get; set; }
        public long? AreaId { get; set; }
    }

    public class SalaryStructureSysInputDTO
    {
        public long? Id { get; set; }
        public string ColName { get; set; }
        public string Name { get; set; }
        public int? SalaryTypeId { get; set; }
        public int? ElementId { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsCalculate { get; set; }
        public bool? IsImport { get; set; }
        public int? Orders { get; set; }
        public long? AreaId { get; set; }
    }

}
