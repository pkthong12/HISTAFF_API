using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using System.Text.RegularExpressions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class SettingMapRepository : RepositoryBase<SYS_SETTING_MAP>, ISettingMapRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public SettingMapRepository(ProfileDbContext context) : base(context)
        {

        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<SettingMapDTO>> GetAll(SettingMapDTO param)
        {
            try
            {
                var queryable = from p in _appContext.SettingMaps
                                join o in _appContext.Organizations on p.ORG_ID equals o.ID
                                where p.IS_ACTIVE == true
                                orderby o.NAME
                                select new SettingMapDTO
                                {
                                    Id = p.ID,
                                    Address = p.ADDRESS,
                                    Radius = p.RADIUS,
                                    Lat = p.LAT,
                                    Long = p.LNG,
                                    Zoom = p.ZOOM,
                                    Center = p.CENTER,
                                    Ip = p.IP,
                                    BssId = p.BSSID,
                                    OrgId = p.ORG_ID,
                                    OrgName = o.NAME,
                                    QRCode = p.QRCODE,
                                    Name = p.NAME
                                };

                if (!String.IsNullOrWhiteSpace(param.Address))
                {
                    queryable = queryable.Where(p => p.Address.ToUpper().Contains(param.Address.ToUpper()));
                }

                var orgIds = await QueryData.ExecuteList(Procedures.PKG_COMMON_LIST_ORG,
                     new
                     {
                         P_IS_ADMIN = _appContext.IsAdmin == true ? 1 : 0,
                         
                         P_ORG_ID = param.OrgId,
                         P_CURENT_USER_ID = _appContext.CurrentUserId,
                         P_CUR = QueryData.OUT_CURSOR
                     }, false);


                List<long?> ids = orgIds.Select(c => (long?)((dynamic)c).ID).ToList();
                if (param.OrgId != null)
                {
                    ids.Add(param.OrgId);
                }
                queryable = queryable.Where(p => ids.Contains(p.OrgId));
                return await PagingList(queryable, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                var r = await (from p in _appContext.SettingMaps
                               where p.ID == id 
                               select new
                               {
                                   Id = p.ID,
                                   Ip = p.IP,
                                   Address = p.ADDRESS,
                                   Radius = p.RADIUS,
                                   Lat = p.LAT,
                                   Lng = p.LNG,
                                   Zoom = p.ZOOM,
                                   Center = p.CENTER,
                                   Name = p.NAME
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Create Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateAsync(SettingMapInputDTO param)
        {
            try
            {


                var data = Map(param, new SYS_SETTING_MAP());
                data.IS_ACTIVE = true;
                data.QRCODE = Guid.NewGuid().ToString();
                var result = await _appContext.SettingMaps.AddAsync(data);
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex);
            }
        }
        /// <summary>
        /// CMS Edit Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> UpdateAsync(SettingMapInputDTO param)
        {
            try
            {
                var a = _appContext.SettingMaps.Where(x => x.ID == param.Id ).FirstOrDefault();
                if (a == null)
                {
                    return new ResultWithError(404);
                }

                var data = Map(param, a);
                data.IS_ACTIVE = true;
                if (a.QRCODE == null)
                {
                    data.QRCODE = Guid.NewGuid().ToString();
                }
                var result = _appContext.SettingMaps.Update(data);

                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex);
            }
        }



        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var r = _appContext.SettingMaps.Where(x => x.ID == item ).FirstOrDefault();
                    if (r == null)
                    {
                        return new ResultWithError(404);
                    }
                    r.IS_ACTIVE = !r.IS_ACTIVE;
                    var result = _appContext.SettingMaps.Update(r);
                }
                await _appContext.SaveChangesAsync();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// Get List Group is Activce
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> GetList()
        {
            try
            {
                var queryable = from p in _appContext.SettingMaps
                                where p.IS_ACTIVE == true 
                                orderby p.ID
                                select new { p };
                return new ResultWithError(queryable.ToList());
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }


        /// <summary>
        /// GetIP
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ResultWithError GetIP()
        {
            try
            {
                //string url = "http://checkip.dyndns.org";
                //System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                //System.Net.WebResponse resp = req.GetResponse();
                //System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                //string response = sr.ReadToEnd().Trim();
                //int first = response.IndexOf("Address: ") + 9;
                //int last = response.LastIndexOf("</body>");

                string externalIP;
                externalIP = (new System.Net.WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();

                var obj = new WifiParam();
                obj.Ip = externalIP;
                return new ResultWithError(obj);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }

        /// <summary>
        /// GetIP
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ResultWithError GetBSSID()
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.FileName = "cmd";
                proc.StartInfo.Arguments = @"/C ""netsh wlan show networks mode=bssid | findstr BSSID """;

                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                var obj = new WifiParam();
                obj.BssId = output;
                return new ResultWithError(obj);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
