using Dapper;
using Order.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Order.Core.OrderResult;

namespace Order.SqlServices
{
    public class DataQuery
    {
        public OrderResults<IEnumerable<OrderDataVM>> GetOrderData(int id)
        {
            var dynamicParams = new DynamicParameters();//←動態參數
            dynamicParams.Add("OrderID", id);

            var result = new OrderResults<IEnumerable<OrderDataVM>>();

            SqlConnection conn = new SqlConnection("Data Source=howardorder.database.windows.net;Initial Catalog=OrderDatabase;Persist Security Info=True;User Id =howard;Password=Yihao1222");
            conn.Open();

            var SQL = conn.Query<OrderDataVM>(
                @"Select Ord.OrderID, Ord.ShipName , OD.ProductID, PD.ProductName, EP.EmployeeID, EP.LastName + ' ' + EP.FirstName as 'EmployeeName', Ord.CustomerID, CT.CompanyName, CT.ContactName
                    From Orders Ord
                    join Employees EP on Ord.EmployeeID = EP.EmployeeID
                    join Customers CT on Ord.CustomerID = CT.CustomerID
                    join [Order Details] OD on Ord.OrderID = OD.OrderID
                    join Products PD on OD.ProductID = PD.ProductID
                    where Ord.OrderID = @OrderID
                    Group By Ord.OrderID, Ord.ShipName ,OD.ProductID ,PD.ProductName , EP.EmployeeID, EP.LastName, EP.FirstName, Ord.CustomerID, CT.CompanyName, CT.ContactName"
                , dynamicParams);

            conn.Close();
            result.Payload = SQL;


            return result;
        }
    }
}
