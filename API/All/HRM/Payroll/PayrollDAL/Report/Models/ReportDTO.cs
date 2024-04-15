using Common.Paging;

namespace PayrollDAL.ViewModels
{
    public class ReportDTO : Pagings
    {
        public long Id { get; set; }
        public int? PeriodId { get; set; }
        public int? OrgId { get; set; }
        public string OrgName { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }
        public string Note { get; set; }
        public int Year { get; set; }
        public string Period { get; set; }
        public DateTime? AdvanceDate { get; set; }  //Ngày ứng lương
        public long Money { get; set; } // Số tiền ứng
        public long? StatusId { get; set; } // Trạng thái
        public string StatusName { get; set; }
        public string SignerName { get; set; } // Tên người ký
        public string SignerPosition { get; set; } // Chức danh người ký
        public DateTime? SignDate { get; set; }

        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class ThuNhapBinhQuanObject
    {
        public string STT { set; get; }
        public string OrgNameA { set; get; }// org cột B
        public string OrgNameB { set; get; }// org cột C
        public string ORG_NAME2 { set; get; }
        public string ORG_NAME3 { set; get; }
        public string ORG_NAME4 { set; get; }
        public decimal? SL_NAM_TRC { set; get; }
        public decimal? SL_NAM_NAY { set; get; }
        public decimal? LUONG_NAM_TRC { set; get; }
        public decimal? LUONG_NAM_NAY { set; get; }
        public decimal? IS_PARENT { set; get; }
    }

    public class ThuNhapBinhQuanObjectExport
    {
        public string STT { set; get; }
        public string OrgNameA { set; get; }// org cột B
        public string OrgNameB { set; get; }// org cột C

        public decimal? SL_NAM_TRC { set; get; }
        public decimal? SL_NAM_NAY { set; get; }
        public decimal? PT_TANG_GIAM_NS { set; get; }
        public decimal? SL_TANG_GIAM { set; get; }

        public decimal? LUONG_NAM_TRC { set; get; }
        public decimal? LUONG_NAM_NAY { set; get; }
        public decimal? PT_TANG_GIAM_LUONG { set; get; }

        public decimal? BQ_NAM_TRC { set; get; }
        public decimal? BQ_NAM_NAY { set; get; }
        public decimal? PT_TANG_GIAM_THUNHAP { set; get; }
        public decimal? IS_PARENT { set; get; }
    }

    public class CoCauNhanSuObject
    {
        public string STT { set; get; }
        public string OrgNameA { set; get; }// org cột B
        public string OrgNameB { set; get; }// org cột C
        public string ORG_NAME2 { set; get; }
        public string ORG_NAME3 { set; get; }
        public string ORG_NAME4 { set; get; }
        public decimal? TRENDH { set; get; }
        public decimal? DH { set; get; }
        public decimal? CD { set; get; }
        public decimal? CN { set; get; }
        public decimal? NAM { set; get; }
        public decimal? NU { set; get; }
        public decimal? TANG { set; get; }
        public decimal? TANGMOI { set; get; }
        public decimal? TANGKHAC { set; get; }
        public decimal? DCNBTANG { set; get; }
        public decimal? TANGNOIBO { set; get; }
        public decimal? GIAM { set; get; }
        public decimal? XINTHOIVIEC { set; get; }
        public decimal? SATHAI { set; get; }
        public decimal? NGHIHUU { set; get; }
        public decimal? NGHIKHAC { set; get; }
        public decimal? GIAMNOIBO { set; get; }



    }

    public class CoCauNhanSuExport
    {
        public string STT { set; get; }
        public string OrgNameA { set; get; }// org cột B
        public string OrgNameB { set; get; }// org cột C

        public decimal? TRENDH { set; get; }
        public decimal? DH { set; get; }
        public decimal? CD { set; get; }
        public decimal? CN { set; get; }
        public decimal? NAM { set; get; }
        public decimal? NU { set; get; }
        public decimal? TANG { set; get; }
        public decimal? TANGMOI { set; get; }
        public decimal? TANGKHAC { set; get; }
        public decimal? DCNBTANG { set; get; }
        public decimal? TANGNOIBO { set; get; }
        public decimal? GIAM { set; get; }
        public decimal? XINTHOIVIEC { set; get; }
        public decimal? SATHAI { set; get; }
        public decimal? NGHIHUU { set; get; }
        public decimal? NGHIKHAC { set; get; }
        public decimal? GIAMNOIBO { set; get; }
        public decimal? IS_PARENT { set; get; }

    }

    public class TongHopQuyLuongObject
    {
        public string STT { set; get; }
        public string OrgNameA { set; get; }// org cột B
        public string OrgNameB { set; get; }// org cột C
        public string ORG_NAME2 { set; get; }
        public string ORG_NAME3 { set; get; }
        public string ORG_NAME4 { set; get; }
        public decimal? SOLDTHANGTRC { set; get; }
        public decimal? TONGLUONGTHANGTRC { set; get; }
        public decimal? SOLDTHANGNAY { set; get; }
        public decimal? TQLCOSO { set; get; }
        public decimal? TQLTHUCTRA { set; get; }
        public decimal? HESOTHUNHAP { set; get; }
        public decimal? SNTHAMGIABAOHIEM { set; get; }
        public decimal? TONGLUONGBHXH { set; get; }




    }
    public class TongHopQuyLuongExport
    {
        public string STT { set; get; }
        public string OrgNameA { set; get; }// org cột B
        public string OrgNameB { set; get; }// org cột C

        public decimal? SOLDTHANGTRC { set; get; }
        public decimal? TONGLUONGTHANGTRC { set; get; }
        public decimal? SOLDTHANGNAY { set; get; }
        public decimal? TQLCOSO { set; get; }
        public decimal? TQLTHUCTRA { set; get; }
        public decimal? TNBQ { set; get; }

        public decimal? HESOTHUNHAP { set; get; }
        public decimal? SLDTANG { set; get; }
        public decimal? TONGQUYLUONGTANG { set; get; }
        public decimal? SNTHAMGIABAOHIEM { set; get; }
        public decimal? TONGLUONGBHXH { set; get; }
        public decimal? TYLE1 { set; get; }
        public decimal TYLE2 { set; get; }

    }
    public class CCNSParam
    {
        public int? OrgId { set; get; }
        public int? PeriodId { set; get; }
        public DateTime? Time { set; get; }
    }
}
