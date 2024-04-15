using Common.Paging;

namespace ProfileDAL.ViewModels
{
    public class SettingMapDTO : Pagings
    {
        public long Id { get; set; }
        public long? OrgId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal? Radius { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public int? Zoom { get; set; }
        public string Center { get; set; }
        public string Ip { get; set; }
        public string BssId { get; set; }
        public string OrgName { get; set; }
        public string QRCode { get; set; }
    }


    public class SettingMapInputDTO
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal? Radius { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public int? Zoom { get; set; }
        public string Center { get; set; }
        public int? OrgId { get; set; }
        public string Ip { get; set; }
        public string BssId { get; set; }
    }
    public class WifiParam
    {
        public string Ip { get; set; }
        public string BssId { get; set; }
    }
}
