using System.Data;
using System.Dynamic;
using API.All.DbContexts;
using Common.Paging;

namespace Common.Interfaces
{
    public abstract class AbsQueryDataTemplate
    {
        public Dictionary<string, int> arrOutType;
        public string OUT_CURSOR = "OUT_CURSOR";
        public string OUT_NUMBER = "OUT_NUMBER";
        public string OUT_STRING = "OUT_STRING";
        public string OUT_DATE = "OUT_DATE";
        public IDbConnection connet;       
        // public TLAContext _tlaContext;
        public DbContextBase _dbContextBase;
        // <summary>
        // Thực hiện query 
        // </summary>
        // <param name="sql">Câu lệnh SQL</param>
        // <returns>Trả về giá trị của câu lệnh</returns>
        public abstract Task<object> ExecuteStoreQuery(string sql);
        // <summary>
        // Query procedure có chứa tham số
        // </summary>
        // <param name="sql">Câu lệnh SQL</param>
        // <param name="obj">Đối tượng chưa parameter</param>
        // <returns>Dữ liệu trả về Entity</returns>
        public abstract Task<List<T>> ExecuteStore<T>(string sql, object obj) where T : new();
        // <summary>
        // Query procedure có chứa tham số
        // </summary>
        // <param name="sql">Câu lệnh SQL</param>
        // <param name="obj">Đối tượng chưa parameter</param>
        // <returns>Dữ liệu trả về Entity</returns>
        public abstract Task<List<T>> ExecuteStore<T>(string sql, object obj, bool? haveUserTenant) where T : new();
        public abstract Task<List<T>> ExecuteStorePortal<T>(string sql, object obj) where T : new();
        public abstract Task<string> ExecuteStoreString(string sql, object obj);
        // <summary>
        // Query procedure có pagging
        // </summary>
        // <param name="sql">Câu lệnh SQL</param>
        // <param name="obj">Đối tượng chưa parameter</param>
        // <returns>Dữ liệu trả về</returns>
        public abstract Task<PagedResult<T>> ExecuteStorePage<T>(string sql, object obj) where T : new();
        /// <summary>
        ///  Đọc nhiều cursor 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract Task<List<List<ExpandoObject>>> ExecuteStoreObject(string sql, object obj, bool? haveUserTenant);
        /// <summary>
        /// Return 1 query Store có phân trang 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract Task<PagedResult<ExpandoObject>> ExecutePaging(string sql, object obj, bool? haveUserTenant);
        /// <summary>
        ///  ExecuteNonQuery Add/Edit/ delete  
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract Task<int> ExecuteNonQuery(string sql);
        /// <summary>
        ///  ExecuteNonQuery Add/Edit/ delete  
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract Task<int> ExecuteNonQuery(string sql, object obj, bool? haveUserTenant);
        public abstract Task<List<ExpandoObject>> ExecuteList(string sql, object obj, bool? haveUserTenant);
        public abstract Task<ExpandoObject> ExecuteObject(string sql, object obj, bool? haveUserTenant);
        public abstract Task Execute(string sql, object obj, bool? haveUserTenant);
        /// <summary>
        ///  Đọc nhiều cursor, với ConnectionString cụ thể
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract Task<List<List<ExpandoObject>>> ExecuteStoreObject(string conect, string sql, object obj);

        public abstract Task Execute(string conect, string sql, object obj);
        public abstract Task<List<ExpandoObject>> ExecuteList(string conect, string sql, object obj);
        // <summary>
        // Query procedure có chứa tham số
        // </summary>
        // <param name="sql">Câu lệnh SQL</param>
        // <param name="obj">Đối tượng chưa parameter</param>
        // <returns>Dữ liệu trả về</returns>
        public abstract DataSet ExecuteStoreToTable(string sql, object obj, bool? haveUserTenant);
        /// <summary>
        /// Map DataTable to List by Order column 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public abstract List<T> ConvertToListByOrder<T>(DataTable dt);
        // <summary>
        // Gán type database tương ứng với value
        // </summary>
        // <param name="sKey">Tên parameter</param>
        // <param name="sValue">Giá trị của parameter</param>
        // <returns></returns>
        // <remarks></remarks>
        // private Object GetParameter(string sKey, Object sValue, ref bool bOut);
        // private List<T> MapToList<T>(Object dr) where T : new();
    }
}