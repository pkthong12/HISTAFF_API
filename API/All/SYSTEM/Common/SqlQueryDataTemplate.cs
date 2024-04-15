﻿using Common.Paging;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Dynamic;
using System.Reflection;
using Common.Interfaces;
using API.All.DbContexts;
using API;

namespace Common.DataAccess
{
    public class SqlQueryDataTemplate : AbsQueryDataTemplate
    {
        protected string connectionString;
        public SqlQueryDataTemplate(DbContextBase dbContextBase)
        {
            arrOutType = new Dictionary<string, int>();
            arrOutType.Add("OUT_CURSOR", 0);
            arrOutType.Add("OUT_NUMBER", 1);
            arrOutType.Add("OUT_STRING", 2);
            arrOutType.Add("OUT_DATE", 3);
            connet = dbContextBase.Database.GetDbConnection();
            _dbContextBase = dbContextBase;
            connectionString = _dbContextBase._appSettings.ConnectionStrings.CoreDb;
     

        }

        public override async Task<object> ExecuteStoreQuery(string sql)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);

            conn.Open();

            cmd.CommandType = CommandType.Text;

            // object lst = cmd.ExecuteScalar();
            object lst = await cmd.ExecuteScalarAsync();
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
            return lst;
        }
        // <summary>
        // Query procedure có chứa tham số
        // </summary>
        // <param name="sql">Câu lệnh SQL</param>
        // <param name="obj">Đối tượng chưa parameter</param>
        // <returns>Dữ liệu trả về Entity</returns>
        public override async Task<List<T>> ExecuteStore<T>(string sql, object obj)
        {
            string strPosOut = "";
            SqlConnection conn = new SqlConnection(connectionString);
            var _sql = sql.Replace(".", "_");
            SqlCommand cmd = new SqlCommand(_sql, conn);
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            // Add parameter
            if (obj != null)
            {
                foreach (PropertyInfo info in obj.GetType().GetProperties())
                {
                    int bOut = 0;
                    var para = GetParameter(info.Name, info.GetValue(obj, null), ref bOut);
                    if (para != null)
                    {
                        if (bOut == 1)
                            strPosOut += info.Name + ";";
                        if (bOut == -99)
                            continue;
                        cmd.Parameters.Add(para);
                    }
                }
            }
            List<T> lst;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                lst = MapToList<T>((SqlDataReader)reader);
            }
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
            return lst;
        }

        // <summary>
        // Query procedure có chứa tham số
        // </summary>
        // <param name="sql">Câu lệnh SQL</param>
        // <param name="obj">Đối tượng chưa parameter</param>
        // <returns>Dữ liệu trả về Entity</returns>
        public override async Task<List<T>> ExecuteStore<T>(string sql, object obj, bool? haveUserTenant)
        {
            string strPosOut = "";
            SqlConnection conn = new SqlConnection(connectionString);
            var _sql = sql.Replace(".", "_");
            SqlCommand cmd = new SqlCommand(_sql, conn);
            conn.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            // add param 
            if (haveUserTenant != null && haveUserTenant == true)
            {

                SqlParameter User = new SqlParameter();
                User.ParameterName = "P_USER_ID";
                User.DbType = DbType.String;
                User.Value = _dbContextBase.CurrentUserId;
                cmd.Parameters.Add(User);
            }
            // Add parameter
            if (obj != null)
            {
                foreach (PropertyInfo info in obj.GetType().GetProperties())
                {
                    int bOut = 0;
                    var para = GetParameter(info.Name, info.GetValue(obj, null), ref bOut);
                    if (para != null)
                    {
                        if (bOut == 1)
                            strPosOut += info.Name + ";";
                        if (bOut == -99)
                            continue;
                        cmd.Parameters.Add(para);
                    }
                }
            }
            List<T> lst;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                lst = MapToList<T>((SqlDataReader)reader);
            }
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
            return lst;
        }
        public override async Task<List<T>> ExecuteStorePortal<T>(string sql, object obj)
        {
            string strPosOut = "";
            SqlConnection conn = new SqlConnection(connectionString);
            var _sql = sql.Replace(".", "_");
            SqlCommand cmd = new SqlCommand(_sql, conn);
            conn.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            // add param 

            SqlParameter User = new SqlParameter();
            User.ParameterName = "P_EMP_ID";
            User.DbType = DbType.Int64;
            User.Value = _dbContextBase.EmpId;
            cmd.Parameters.Add(User);
            // Add parameter
            if (obj != null)
            {
                foreach (PropertyInfo info in obj.GetType().GetProperties())
                {
                    int bOut = 0;
                    var para = GetParameter(info.Name, info.GetValue(obj, null), ref bOut);
                    if (para != null)
                    {
                        if (bOut == 1)
                            strPosOut += info.Name + ";";
                        if (bOut == -99)
                            continue;
                        cmd.Parameters.Add(para);
                    }
                }
            }
            List<T> lst;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                lst = MapToList<T>((SqlDataReader)reader);
            }
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
            return lst;
        }

        public override async Task<string> ExecuteStoreString(string sql, object obj)
        {
            string strPosOut = "";
            SqlConnection conn = new SqlConnection(connectionString);
            var _sql = sql.Replace(".", "_");
            SqlCommand cmd = new SqlCommand(_sql, conn);
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter { ParameterName = "@P_USER_ID", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = _dbContextBase.CurrentUserId });
            // Add parameter
            if (obj != null)
            {
                foreach (PropertyInfo info in obj.GetType().GetProperties())
                {
                    int bOut = 0;
                    var para = GetParameter(info.Name, info.GetValue(obj, null), ref bOut);
                    if (para != null)
                    {
                        if (bOut == 1)
                            strPosOut += info.Name + ";";
                        if (bOut == -99)
                            continue;
                        cmd.Parameters.Add(para);
                    }
                }
            }
            string lst = "";
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (reader != null && reader.HasRows)
                {
                    reader.Read();
                    lst = reader.GetString(0);
                    reader.Close();
                }
            }

            cmd.Dispose();
            conn.Close();
            conn.Dispose();
            return lst;
        }
        // <summary>
        // Query procedure có pagging
        // </summary>
        // <param name="sql">Câu lệnh SQL</param>
        // <param name="obj">Đối tượng chưa parameter</param>
        // <returns>Dữ liệu trả về</returns>
        public override async Task<PagedResult<T>> ExecuteStorePage<T>(string sql, object obj)
        {
            string strPosOut = "";
            SqlConnection conn = new SqlConnection(connectionString);
            var _sql = sql.Replace(".", "_");
            SqlCommand cmd = new SqlCommand(_sql, conn);
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter
            var p = obj.GetType();
            foreach (PropertyInfo info in p.GetProperties())
            {
                int bOut = 0;
                var para = GetParameter(info.Name, info.GetValue(obj, null), ref bOut);
                if (para != null)
                {
                    if (bOut == 1)
                        strPosOut += info.Name + ";";
                    if (bOut == -99)
                        continue;
                    cmd.Parameters.Add(para);
                }
            }

            List<T> lst;
            long totalRecord = 0;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                lst = MapToList<T>((SqlDataReader)reader);
                reader.NextResult();
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        totalRecord = reader.GetInt64(0);
                    }
                }
            }
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
            var pageNO = (int)obj.GetType().GetProperty("p_page_no").GetValue(obj, null);
            var pageSize = (int)obj.GetType().GetProperty("p_page_size").GetValue(obj, null);
            return new PagedResult<T>(lst, pageNO, pageSize, totalRecord);
        }


        public override async Task<List<List<ExpandoObject>>> ExecuteStoreObject(string sql, object obj, bool? haveUserTenant)
        {
            string strPosOut = "";
            using SqlConnection conn = new SqlConnection(connectionString);
            var _sql = sql.Replace(".", "_");
            using SqlCommand cmd = new SqlCommand(_sql, conn);
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;

            // add param 
            if (haveUserTenant != null && haveUserTenant == true)
            {

                SqlParameter User = new SqlParameter();
                User.ParameterName = "@P_CURENT_USER_ID";
                User.DbType = DbType.String;
                User.Value = _dbContextBase.CurrentUserId;
                cmd.Parameters.Add(User);
            }

            // Add parameter
            var p = obj.GetType();
            foreach (PropertyInfo info in p.GetProperties())
            {
                int bOut = 0;
                var para = GetParameter(info.Name, info.GetValue(obj, null), ref bOut);
                if (para != null)
                {
                    if (bOut == 1)
                        strPosOut += info.Name + ";";
                    if (bOut == -99)
                        continue;
                    cmd.Parameters.Add(para);
                }
            }

            List<List<ExpandoObject>> res = new List<List<ExpandoObject>>();
            try
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    do
                    {
                        List<ExpandoObject> lst = new List<ExpandoObject>();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var expandoObject = new ExpandoObject() as IDictionary<string, object>;
                                for (var i = 0; i < reader.FieldCount; i++)
                                    expandoObject.Add(reader.GetName(i), reader[i]);
                                lst.Add(expandoObject as ExpandoObject);
                            };
                        }

                        res.Add(lst);

                    } while (reader.NextResult());

                }
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public override async Task<PagedResult<ExpandoObject>> ExecutePaging(string sql, object obj, bool? haveUserTenant)
        {
            try
            {
                var data = await ExecuteStoreObject(sql, obj, haveUserTenant);
                int pageNO = 0;
                int pageSize = 0;
                if (obj.GetType().GetProperty("p_page_no") != null)
                {
                    pageNO = (int)obj.GetType().GetProperty("p_page_no").GetValue(obj, null);
                }
                if(obj.GetType().GetProperty("p_page_size") != null)
                {
                    pageSize = (int)obj.GetType().GetProperty("p_page_size").GetValue(obj, null);
                }
                ExpandoObject t = data[1].First();
                //lay cur so 0 va cur 1
                var res = new PagedResult<ExpandoObject>(data[0], pageNO, pageSize, long.Parse(t.First().Value.ToString()));
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ExecuteNonQuery Add/Edit/ delete  
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override async Task<int> ExecuteNonQuery(string sql)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Connection = conn;
            conn.Open();

            cmd.CommandType = CommandType.Text;

            // object lst = cmd.ExecuteScalar();
            var rowEffect = await cmd.ExecuteNonQueryAsync();
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
            return rowEffect;
        }

        /// <summary>
        ///  ExecuteNonQuery Add/Edit/ delete  
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override async Task<int> ExecuteNonQuery(string sql, object obj, bool? haveUserTenant)
        {
            string strPosOut = "";
             SqlConnection conn = new SqlConnection(connectionString);
            var _sql = sql.Replace(".", "_");
            SqlCommand cmd = new SqlCommand(_sql, conn);
            conn.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // add param 
            if (haveUserTenant != null && haveUserTenant == true)
            {

                SqlParameter User = new SqlParameter();
                User.ParameterName = "P_USER_ID";
                User.DbType = DbType.String;
                User.Value = _dbContextBase.CurrentUserId;
                cmd.Parameters.Add(User);
            }

            // Add parameter
            var p = obj.GetType();
            foreach (PropertyInfo info in p.GetProperties())
            {
                int bOut = 0;
                var para = GetParameter(info.Name, info.GetValue(obj, null), ref bOut);
                if (para != null)
                {
                    if (bOut == 1)
                        strPosOut += info.Name + ";";
                    if (bOut == -99)
                        continue;
                    cmd.Parameters.Add(para);
                }
            }

            var r = await cmd.ExecuteNonQueryAsync();
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
            return r;
        }


        public override async Task<List<ExpandoObject>> ExecuteList(string sql, object obj, bool? haveUserTenant)
        {
            try
            {
                var data = await ExecuteStoreObject(sql, obj, haveUserTenant);
                return data[0];
            }
            catch
            {
                throw;
            }
        }

        public override async Task<ExpandoObject> ExecuteObject(string sql, object obj, bool? haveUserTenant)
        {
            try
            {
                var data = await ExecuteStoreObject(sql, obj, haveUserTenant);
                if (data != null && data.Count > 0)
                {
                    if (data[0].Count > 0)
                    {
                        return data[0].First();
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override async Task Execute(string sql, object obj, bool? haveUserTenant)
        {
            await ExecuteStoreObject(sql, obj, haveUserTenant);

        }

        /// <summary>
        ///  Đọc nhiều cursor, với ConnectionString cụ thể
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override async Task<List<List<ExpandoObject>>> ExecuteStoreObject(string conect, string sql, object obj)
        {
            string strPosOut = "";
            var conectionString = _dbContextBase._config[conect];
            SqlConnection conn = new SqlConnection(conectionString);
            var _sql = sql.Replace(".", "_");
            SqlCommand cmd = new SqlCommand(_sql, conn);
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter
            var p = obj.GetType();
            foreach (PropertyInfo info in p.GetProperties())
            {
                int bOut = 0;
                var para = GetParameter(info.Name, info.GetValue(obj, null), ref bOut);
                if (para != null)
                {
                    if (bOut==1)
                        strPosOut += info.Name + ";";
                    if (bOut == -99)
                        continue;
                    cmd.Parameters.Add(para);
                }
            }
            List<List<ExpandoObject>> res = new List<List<ExpandoObject>>();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                do
                {
                    List<ExpandoObject> lst = new List<ExpandoObject>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var expandoObject = new ExpandoObject() as IDictionary<string, object>;

                            for (var i = 0; i < reader.FieldCount; i++)
                                expandoObject.Add(reader.GetName(i), reader[i]);
                            lst.Add(expandoObject as ExpandoObject);
                        };
                    }

                    res.Add(lst);

                } while (reader.NextResult());

            }
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
            return res;
        }
        public override async Task Execute(string conect, string sql, object obj)
        {
            await ExecuteStoreObject(conect, sql, obj);

        }
        public override async Task<List<ExpandoObject>> ExecuteList(string conect, string sql, object obj)
        {
            try
            {
                var data = await ExecuteStoreObject(conect, sql, obj);
                return data[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // <summary>
        // Query procedure có chứa tham số
        // </summary>
        // <param name="sql">Câu lệnh SQL</param>
        // <param name="obj">Đối tượng chưa parameter</param>
        // <returns>Dữ liệu trả về</returns>
        public override DataSet ExecuteStoreToTable(string sql, object obj, bool? haveUserTenant)
        {
            string strPosOut = "";
            SqlConnection conn = new SqlConnection(connectionString);
            var _sql = sql.Replace(".", "_");
            SqlCommand cmd = new SqlCommand(_sql, conn);
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;

            // add param 
            if (haveUserTenant != null && haveUserTenant == true)
            {

                SqlParameter User = new SqlParameter();
                User.ParameterName = "@P_USER_ID";
                User.Value = _dbContextBase.CurrentUserId;
                User.DbType = DbType.String;

                cmd.Parameters.Add(User);
            }

            // Add parameter
            var p = obj.GetType();
            foreach (PropertyInfo info in p.GetProperties())
            {
                int bOut = 0;
                var para = GetParameter(info.Name, info.GetValue(obj, null), ref bOut);
                if (para != null)
                {
                    if (bOut==1)
                        strPosOut += info.Name + ";";
                    if (bOut == -99)
                        continue;
                    cmd.Parameters.Add(para);
                }
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            da.Fill(ds);
            // Lấy dữ liệu kiểu out để trả về

            // Dispose all resource
            da.Dispose();
            cmd.Dispose();
            conn.Close();
            conn.Dispose();
            return ds;
        }



        /// <summary>
        /// Map DataTable to List by Order column 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public override List<T> ConvertToListByOrder<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            dt.Columns[0].ColumnName = "CODE";
            dt.DefaultView.RowFilter = "CODE IS NOT NULL or CODE <> ''";
            var dt1 = dt.DefaultView.ToTable();

            return dt1.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                var length = properties.Length < dt.Columns.Count ? properties.Length : dt.Columns.Count;
                for (int i = 0; i < length; i++)
                {
                    var pro = properties[i];
                    try
                    {
                        var type = pro.PropertyType;
                        var sValue = row[i];
                        pro.SetValue(objT, Convert.ChangeType(sValue, type));
                    }
                    catch (Exception ex) { }

                }
                return objT;
            }).ToList();
        }
        // <summary>
        // Gán type database tương ứng với value
        // </summary>
        // <param name="sKey">Tên parameter</param>
        // <param name="sValue">Giá trị của parameter</param>
        // <returns></returns>
        // <remarks></remarks>
        private SqlParameter GetParameter(string sKey, Object sValue, ref int bOut)
        {
            SqlParameter para = new SqlParameter();
            para.ParameterName = sKey;
            // 
            if (sValue == null || ((sValue != null & (sValue != DBNull.Value)) && !arrOutType.ContainsKey(sValue.ToString())))
            {
                if (sValue != null)
                {
                    // Kiểm tra type để gán thuộc tính tương ứng
                    switch (sValue.GetType())
                    {
                        case var @case when @case == typeof(bool):
                            {
                                para.DbType = DbType.Int16;
                                if (bool.Parse(sValue.ToString()))
                                    para.Value = -1;
                                else
                                    para.Value = 0;
                                break;
                            }

                        case var case1 when case1 == typeof(DateTime):
                            {
                                para.DbType = DbType.Date;
                                para.Value = sValue;
                                break;
                            }
                        case var case3 when case3 == typeof(double):
                            {
                                para.DbType = DbType.Double;
                                para.Value = sValue;
                                break;
                            }

                        case var case4 when case4 == typeof(decimal):
                            {
                                para.DbType = DbType.Decimal;
                                para.Value = sValue;
                                break;
                            }

                        case var case5 when case5 == typeof(int):
                            {
                                para.DbType = DbType.Int64;
                                para.Value = sValue;
                                break;
                            }

                        case var case6 when case6 == typeof(string):
                            {
                                para.DbType = DbType.String;
                                para.Value = sValue;
                                break;
                            }

                        case var case7 when case7 == typeof(byte[]):
                            {
                                para.DbType = DbType.Binary;
                                if (sValue == DBNull.Value)
                                    para.Size = 8;
                                para.Value = sValue;
                                break;
                            }
                        case var case8 when case8 == typeof(Int64):
                            {
                                para.DbType = DbType.Int64;
                                para.Value = sValue;
                                break;
                            }
                    }
                }
            }
            else if (sValue == DBNull.Value)
            {
                para.Size = 8;
                para.Value = sValue;
            }
            else
            {
                para.Direction = ParameterDirection.Output;
                switch (arrOutType[sValue.ToString()])
                {
                    case 0:
                        {
                            //para.SqlDbType = SqlDbType.RefCursor;
                            bOut = -99;
                            break;
                        }

                    case 1:
                        {
                            bOut = 1;
                            para.SqlDbType = SqlDbType.Decimal;
                            break;
                        }

                    case 2:
                        {
                            bOut = 1;
                            para.SqlDbType = SqlDbType.NVarChar;
                            para.Size = 255;
                            break;
                        }

                    case 3:
                        {
                            bOut = 1;
                            para.SqlDbType = SqlDbType.Date;
                            break;
                        }
                }
            }
            return para;
        }
        private List<T> MapToList<T>(SqlDataReader dr) where T : new()
        {
            var entities = new List<T>();
            if (dr != null && dr.HasRows)
            {
                var entity = typeof(T);

                var propDict = new Dictionary<string, PropertyInfo>();
                var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);
                while (dr.Read())
                {
                    T newObject = new T();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        try
                        {
                            var name = dr.GetName(i).ToUpper().Replace("_", "");
                            if (propDict.ContainsKey(name))
                            {
                                var info = propDict[name];
                                if ((info != null) && info.CanWrite)
                                {
                                    var val = dr.GetValue(i);
                                    if (!DBNull.Value.Equals(val))
                                    {
                                        if (info.PropertyType == typeof(string))
                                        {
                                            info.SetValue(newObject, val.ToString(), null);
                                        }
                                        else
                                        {
                                            info.SetValue(newObject, val, null);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    entities.Add(newObject);
                }

            }
            return entities;
        }
    }
}