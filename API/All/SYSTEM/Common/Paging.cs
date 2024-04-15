namespace Common.Paging
{
    public class Pagings
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? Sort { get; set; }
        public string? keyword { get; set; }
        public int currentPage { get; set; }
    }

    public class PagedResult<T>
    {
        public class PagingInfo
        {
            public int PageNo { get; set; }

            public int PageSize { get; set; }

            public int PageCount { get; set; }

            public long TotalRecordCount { get; set; }

        }
        public List<T> Data { get; private set; }
        //public List<ExpandoObject> Expando { get;  set; }
        public PagingInfo Paging { get; private set; }

        public PagedResult(IEnumerable<T> items, int pageNo, int pageSize, long totalRecordCount)
        {

            Data = new List<T>(items);

            Paging = new PagingInfo
            {
                PageNo = pageNo,
                PageSize = pageSize,
                TotalRecordCount = totalRecordCount,
                PageCount = totalRecordCount > 0
                    ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
                    : 0
            };
        }
    }

    public class PagedResults<T>
    {
        public List<T> Result { get; private set; }
        public PagedResults(IEnumerable<T> items, int curentPage, int pageSize, long totalRecord)
        {

            Result = new List<T>(items);
            count = totalRecord;
          
        }
        public long count { get; set; }
    }

    public class ResponseAPI
    {
        public string message { get; set; }
        public string statusCode { get; set; }
        public dynamic data { get; set; }
    }

    public class ResponseUpFile
    {
        public string message { get; set; }
        public string statusCode { get; set; }
        public List<DetailFile> data { get; set; }
    }
    public class DetailFile
    {
        public string name { get; set; }
        public string url { get; set; }
    }
    public class NotificationModel
    {
        public List<string> Devices { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }

}
