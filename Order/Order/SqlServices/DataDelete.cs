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
            var delete = new SqlServices();
            
            string SQL =
            @"DELETE [OD]
                FROM [Order Details] OD
                join Orders Ord on OD.OrderID = Ord.OrderID 
                WHERE Ord.OrderID = @OrderID;
                DELETE From Orders 
                where OrderID = @OrderID";
            result = delete.SqlDelete(SQL, dynamicParams);
            return result;
        }
    }
}
