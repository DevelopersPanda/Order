using Dapper;
using System.Data.SqlClient;

namespace Order.SqlServices
{
    public class DataDelete
    {
        public string DeleteOrderData(int OrderID)
        {
            string result;
            var dynamicParams = new DynamicParameters();//←動態參數
            dynamicParams.Add("OrderID", OrderID);

            SqlConnection conn = new SqlConnection("Data Source=howardorder.database.windows.net;Initial Catalog=OrderDatabase;Persist Security Info=True;User Id =howard;Password=Yihao1222");
            conn.Open();
            try
            {
                var SQL = conn.Execute(
                @"DELETE [OD]
                    FROM [Order Details] OD
                    join Orders Ord on OD.OrderID = Ord.OrderID 
                    WHERE Ord.OrderID = @OrderID;
                    DELETE From Orders 
                    where OrderID = @OrderID"
                    , dynamicParams);
                result = "Delete Success";
            }
            catch
            {
                result = "Delete failure";
            }
            conn.Close();
            return result;
        }
    }
}
