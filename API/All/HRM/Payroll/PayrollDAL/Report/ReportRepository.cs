using API.All.DbContexts;
using Common.EPPlus;
using Common.Extensions;
using Common.Repositories;
using PayrollDAL.ViewModels;
using System.Data;

namespace PayrollDAL.Repositories
{
    public class ReportRepository : RepositoryBase, IReportRepository
    {
        private PayrollDbContext _appContext => (PayrollDbContext)_context;
        public ReportRepository(PayrollDbContext context) : base(context)
        {

        }

        public async Task<ResultWithError> PA001(ParaInputReport param)
        {
            try
            {
                var r = await QueryData.ExecuteStoreObject("PKG_REPORT.REPORT_PA001",
                new
                {                   
                    P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                    P_YEAR = param.Year,
                    P_ORG_ID = param.OrgId,
                    P_CUR = QueryData.OUT_CURSOR
                }, true);
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> PA002(ParaInputReport param)
        {
            try
            {
                var r =  QueryData.ExecuteStoreToTable(Procedures.PKG_REPORT_RPT_PA002,
                new
                {
                    P_ORG_ID = param.OrgId,
                    P_PERIOD_ID = param.PeriodId,
                    P_SALARY_TYPE = param.SalaryTypeId,
                    P_CUR = QueryData.OUT_CURSOR,
                    P_CUR_TITLE = QueryData.OUT_CURSOR,
                    P_CUR_HEAD = QueryData.OUT_CURSOR
                }, true);

                if (r.Tables[0].Rows.Count <= 0)
                {
                    return new ResultWithError("DATA_EMPTY");
                }
                r.Tables[0].TableName = "Data";
                r.Tables[1].TableName = "Title";
                r.Tables[2].TableName = "head";
                var pathTemp = _appContext._config["urlPA002"];
                var memoryStream = Template.FillTablePayroll(pathTemp, r);
                return new ResultWithError(memoryStream);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }
        }
        public async Task<ResultWithError> ThuNhapBinhQuan(int? orgId)
        {
            var ds = QueryData.ExecuteStoreToTable("PKG_REPORT_HPG.TNBQ",
                new
                {

                    P_ORG_ID = orgId,
                    P_CUR = QueryData.OUT_CURSOR,
                }, true);

            if (ds.Tables[0].Rows.Count <= 0)
            {
                return new ResultWithError("DATA_EMPTY");
            }
            var lastedList = new List<ThuNhapBinhQuanObjectExport>();
            var lstLinq = JsonConvert.DeserializeObject<List<ThuNhapBinhQuanObject>>(JsonConvert.SerializeObject(ds.Tables[0])).AsEnumerable();
            var parentNameList = lstLinq.Select(x => x.ORG_NAME2).Distinct().ToList();
            for (int i = 0; i <= parentNameList.Count - 1; i++)
            {
                var item = parentNameList[i];
                var objSum = new ThuNhapBinhQuanObjectExport { OrgNameA = item };
                objSum.STT = ToRoman(i + 1);
                objSum.SL_NAM_TRC = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.SL_NAM_TRC).Sum();
                objSum.SL_NAM_NAY = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.SL_NAM_NAY).Sum();
                objSum.PT_TANG_GIAM_NS = ((objSum.SL_NAM_NAY - objSum.SL_NAM_TRC) / objSum.SL_NAM_NAY) ;
                objSum.SL_TANG_GIAM = (objSum.SL_NAM_NAY - objSum.SL_NAM_TRC);
                objSum.LUONG_NAM_TRC = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.LUONG_NAM_TRC).Sum();
                objSum.LUONG_NAM_NAY = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.LUONG_NAM_NAY).Sum();
                objSum.PT_TANG_GIAM_LUONG = objSum.LUONG_NAM_NAY != 0 ? ((objSum.LUONG_NAM_TRC - objSum.LUONG_NAM_NAY) / objSum.LUONG_NAM_NAY) : 0;
                objSum.BQ_NAM_TRC = objSum.SL_NAM_TRC != 0 ? objSum.LUONG_NAM_TRC / objSum.SL_NAM_TRC : 0;
                objSum.BQ_NAM_NAY = objSum.SL_NAM_NAY != 0 ? objSum.LUONG_NAM_NAY / objSum.SL_NAM_NAY : 0;
                objSum.PT_TANG_GIAM_THUNHAP = objSum.BQ_NAM_NAY != 0 ? (objSum.BQ_NAM_TRC / objSum.BQ_NAM_NAY) / 100 : 0;
                objSum.IS_PARENT = 1;
                lastedList.Add(objSum);
                var objCon = lstLinq.Where(x => x.ORG_NAME2 == item).ToList();
                for (int j = 0; j <= objCon.Count - 1; j++)
                {
                    var item1 = objCon[j];
                    lastedList.Add(new ThuNhapBinhQuanObjectExport
                    {
                        STT = (j + 1).ToString(),
                        OrgNameA = item1.ORG_NAME3,
                        OrgNameB = item1.ORG_NAME4,
                        SL_NAM_TRC = item1.SL_NAM_TRC,
                        SL_NAM_NAY = item1.SL_NAM_NAY,
                        LUONG_NAM_TRC = item1.LUONG_NAM_TRC,
                        LUONG_NAM_NAY = item1.LUONG_NAM_NAY,
                        IS_PARENT = 0
                    }); ;
                }

            }


            var d = new DataSet();
            var t = new DataTable();
            t = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(lastedList));
            d.Tables.Add(t);
            d.Tables[0].TableName = "detail";
            var pathTemp = _appContext._config["urlTemplateThuNhapBinhQUan"];
            var memoryStream = Template.FillColumnBinhQuan(pathTemp, d);
            return new ResultWithError(memoryStream);
        }

        public async Task<ResultWithError> CoCauNhanSu(CCNSParam param)
        {
            var ds = QueryData.ExecuteStoreToTable("PKG_REPORT_HPG.CCNS",
                new
                {
                    P_ORG_ID = param.OrgId,
                    P_TIME = param.Time,
                    P_CUR = QueryData.OUT_CURSOR,
                }, true);

            if (ds.Tables[0].Rows.Count <= 0)
            {
                return new ResultWithError("DATA_EMPTY");
            }
            var lastedList = new List<CoCauNhanSuExport>();
            var lstLinq = JsonConvert.DeserializeObject<List<CoCauNhanSuObject>>(JsonConvert.SerializeObject(ds.Tables[0])).AsEnumerable();
            var parentNameList = lstLinq.Select(x => x.ORG_NAME2).Distinct().ToList();
            for (int i = 0; i <= parentNameList.Count - 1; i++)
            {
                var item = parentNameList[i];
                var objSum = new CoCauNhanSuExport { OrgNameA = item };
                objSum.STT = ToRoman(i + 1);
                objSum.TRENDH = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.TRENDH).Sum();
                objSum.DH = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.DH).Sum();
                objSum.CD = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.CD).Sum();
                objSum.CN = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.CN).Sum();
                objSum.NAM = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.NAM).Sum();
                objSum.TANG = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.TANG).Sum();
                objSum.TANGMOI = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.TANGMOI).Sum();
                objSum.TANGKHAC = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.TANGKHAC).Sum();
                objSum.DCNBTANG = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.DCNBTANG).Sum();
                objSum.TANGNOIBO = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.TANGNOIBO).Sum();
                objSum.GIAM = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.GIAM).Sum();
                objSum.XINTHOIVIEC = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.XINTHOIVIEC).Sum();
                objSum.SATHAI = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.SATHAI).Sum();
                objSum.NGHIHUU = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.NGHIHUU).Sum();
                objSum.NGHIKHAC = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.NGHIKHAC).Sum();
                objSum.GIAMNOIBO = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.GIAMNOIBO).Sum();
                objSum.IS_PARENT = 1;
                lastedList.Add(objSum);
                var objCon = lstLinq.Where(x => x.ORG_NAME2 == item).ToList();
                for (int j = 0; j <= objCon.Count - 1; j++)
                {
                    var item1 = objCon[j];
                    if (item1.ORG_NAME3 != null)
                    {
                        lastedList.Add(new CoCauNhanSuExport
                        {
                            STT = (j + 1).ToString(),
                            OrgNameA = item1.ORG_NAME3,
                            OrgNameB = item1.ORG_NAME4,
                            TRENDH = item1.TRENDH,
                            DH = item1.DH,
                            CD = item1.CD,
                            CN = item1.CN,
                            NAM = item1.NAM,
                            NU = item1.NU,
                            TANG = item1.TANG,
                            TANGMOI = item1.TANGMOI,
                            TANGKHAC = item1.TANGKHAC,
                            DCNBTANG = item1.DCNBTANG,
                            TANGNOIBO = item1.TANGNOIBO,
                            GIAM = item1.GIAM,
                            XINTHOIVIEC = item1.XINTHOIVIEC,
                            SATHAI = item1.SATHAI,
                            NGHIHUU = item1.NGHIHUU,
                            NGHIKHAC = item1.NGHIKHAC,
                            GIAMNOIBO = item1.GIAMNOIBO,
                            IS_PARENT = 0
                        });
                    }
                }

            }


            var d = new DataSet();
            var t = new DataTable();
            t = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(lastedList));
            d.Tables.Add(t);
            d.Tables[0].TableName = "detail";
            var pathTemp = _appContext._config["urlTemplateCoCauNhanSu"];
            var memoryStream = Template.FillColumnBinhQuan(pathTemp, d);
            return new ResultWithError(memoryStream);
        }

        public async Task<ResultWithError> TongHopQuyLuong(CCNSParam param)
        {
            var ds = QueryData.ExecuteStoreToTable("PKG_REPORT_HPG.THQL",
                new
                {
                    P_ORG_ID = param.OrgId,
                    P_PERIOD = param.PeriodId,
                    P_CUR = QueryData.OUT_CURSOR,
                }, true);

            if (ds.Tables[0].Rows.Count <= 0)
            {
                return new ResultWithError("DATA_EMPTY");
            }
            var lastedList = new List<TongHopQuyLuongExport>();
            var lstLinq = JsonConvert.DeserializeObject<List<TongHopQuyLuongObject>>(JsonConvert.SerializeObject(ds.Tables[0])).AsEnumerable();
            var parentNameList = lstLinq.Select(x => x.ORG_NAME2).Distinct().ToList();
            for (int i = 0; i <= parentNameList.Count - 1; i++)
            {
                var item = parentNameList[i];
                var objSum = new TongHopQuyLuongExport { OrgNameA = item };
                objSum.STT = ToRoman(i + 1);
                objSum.SOLDTHANGTRC = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.SOLDTHANGTRC).Sum();
                objSum.TONGLUONGTHANGTRC = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.TONGLUONGTHANGTRC).Sum();
                objSum.SOLDTHANGNAY = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.SOLDTHANGNAY).Sum();
                objSum.TQLCOSO = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.TQLCOSO).Sum();
                objSum.TQLTHUCTRA = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.TQLTHUCTRA).Sum();
                objSum.HESOTHUNHAP = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.HESOTHUNHAP).Sum();
                objSum.SNTHAMGIABAOHIEM = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.SNTHAMGIABAOHIEM).Sum();
                objSum.TONGLUONGBHXH = lstLinq.Where(x => x.ORG_NAME2 == item).Select(x => x.TONGLUONGBHXH).Sum();

                lastedList.Add(objSum);
                var objCon = lstLinq.Where(x => x.ORG_NAME2 == item).ToList();
                for (int j = 0; j <= objCon.Count - 1; j++)
                {
                    var item1 = objCon[j];
                    if (item1.ORG_NAME3 != null)
                    {
                        lastedList.Add(new TongHopQuyLuongExport
                        {
                            STT = (j + 1).ToString(),
                            OrgNameA = item1.ORG_NAME3,
                            OrgNameB = item1.ORG_NAME4,
                            SOLDTHANGTRC = item1.SOLDTHANGTRC,
                            TONGLUONGTHANGTRC = item1.TONGLUONGTHANGTRC,
                            SOLDTHANGNAY = item1.SOLDTHANGNAY,
                            TQLCOSO = item1.TQLCOSO,
                            TQLTHUCTRA = item1.TQLTHUCTRA,
                            TNBQ = item1.SOLDTHANGNAY != 0 ? item1.TQLTHUCTRA / item1.SOLDTHANGNAY : 0,
                            HESOTHUNHAP = item1.HESOTHUNHAP,
                            SLDTANG = item1.SOLDTHANGNAY + item1.SOLDTHANGTRC,
                            TONGQUYLUONGTANG = item1.TQLTHUCTRA - item1.TONGLUONGTHANGTRC,
                            SNTHAMGIABAOHIEM = item1.SNTHAMGIABAOHIEM,
                            TONGLUONGBHXH = item1.TONGLUONGBHXH,

                        });
                    }
                }

            }


            var d = new DataSet();
            var t = new DataTable();
            t = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(lastedList));
            d.Tables.Add(t);
            d.Tables[0].TableName = "detail";
            var pathTemp = _appContext._config["urlTemplateTongHopQuyLuong"];
            var memoryStream = Template.FillColumnBinhQuan(pathTemp, d);
            return new ResultWithError(memoryStream);
        }
        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }
    }
}
