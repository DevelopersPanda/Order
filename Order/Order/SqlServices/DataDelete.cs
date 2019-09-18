using Dapper;
using Order.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Order.SqlServices
{
    public class DataDelete
    {
        public bool DeleteOrderData(int id)
        {
            var dynamicParams = new DynamicParameters();//←動態參數
            dynamicParams.Add("OrderID", id);

            SqlConnection conn = new SqlConnection("Data Source=howardorder.database.windows.net;Initial Catalog=OrderDatabase;Persist Security Info=True;User Id =howard;Password=Yihao1222");
            conn.Open();

            var SQL = conn.Execute(
                @"DELETE [OD]
                    FROM [Order Details] OD
                    join Orders Ord on OD.OrderID = Ord.OrderID 
                    WHERE Ord.OrderID = @OrderID;
                  DELETE From Orders 
                    where OrderID = @OrderID"
                , dynamicParams);


            conn.Close();


            bool result = true;

            return result;
        }
    }
}
