using Dapper;
using System.Data.SqlClient;
using static Order.Core.OrderResult;

namespace Order.SqlServices
{
    public class SqlServices
    {
        SqlConnection conn = new SqlConnection("Data Source=howardorder.database.windows.net;Initial Catalog=OrderDatabase;Persist Security Info=True;User Id =howard;Password=Yihao1222");
        string result;
        public string SqlInsert(string SqlString, DynamicParameters dynamicParameters)
        {
            conn.Open();
            try
            {
                var SQL = conn.Execute(SqlString, dynamicParameters);
                result = "Insert Success";
            }
            catch
            {
                result = "Insert Failure";
            }
            conn.Close();
            return result;
        }
        public string SqlDelete(string SqlString, DynamicParameters dynamicParameters)
        {
            conn.Open();
            try
            {
                var SQL = conn.Execute(SqlString, dynamicParameters);
                result = "Delete Success";
            }
            catch
            {
                result = "Delete Failure";
            }
            
            conn.Close();
            return result;
        }
        public string SqlUpdate(string SqlString, DynamicParameters dynamicParameters)
        {
            conn.Open();
            try
            {
                var SQL = conn.Execute(SqlString, dynamicParameters);
                result = "Update Success";
            }
            catch
            {
                result = "Update Failure";
            }
            conn.Close();
            return result;
        }
    }
}
