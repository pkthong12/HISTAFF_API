using Common.Paging;

namespace AttendanceDAL.ViewModels
{
    public class TimeSheetFomulaDTO : Pagings
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ColName { get; set; }
        public string FormulaName { get; set; }
        public int? Orders { get; set; }
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
    public class TimeSheetFomulaInputDTO
    {
        public long Id { get; set; }

        public string FormulaName { get; set; }


    }

}
