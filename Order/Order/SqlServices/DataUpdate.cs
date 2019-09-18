using Dapper;
using Order.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Order.SqlServices
{
    public class DataUpdate
    {
        public bool UpdateOrderData(updateOrders UpdateData)
        {
            if (UpdateData.OrderID != 0)
            {
                var dynamicParams = new DynamicParameters();//←動態參數
                dynamicParams.Add("OrderID", UpdateData.OrderID);
                var sqlCondition = @"";

                SqlConnection conn = new SqlConnection("Data Source=howardorder.database.windows.net;Initial Catalog=OrderDatabase;Persist Security Info=True;User Id =howard;Password=Yihao1222");
                conn.Open();

                if (UpdateData.EmployeeID.HasValue)
                {
                    sqlCondition = sqlCondition + " EmployeeID = @EmployeeID,";
                    dynamicParams.Add("EmployeeID", UpdateData.EmployeeID);
                }
                if(UpdateData.CustomerID != null)
                {
                    dynamicParams.Add("CustomerID", UpdateData.CustomerID);
                    var queryCustomer = conn.Query<updateOrders>("select CustomerID From Customers where CustomerID = @CustomerID", dynamicParams);
                    if (queryCustomer.Count() != 0)
                    {
                        sqlCondition = sqlCondition + "CustomerID = @CustomerID,";
                        
                    }else
                    {
                        return false;
                    }
                }
                if (UpdateData.ShipVia.HasValue)
                {
                    sqlCondition = sqlCondition + "ShipVia = @ShipVia,";
                    dynamicParams.Add("ShipVia", UpdateData.ShipVia);
                }
                if (UpdateData.Freight.HasValue)
                {
                    sqlCondition = sqlCondition + "Freight = @Freight,";
                    dynamicParams.Add("Freight", UpdateData.Freight);
                }
                if (UpdateData.ShipName != null)
                {
                    sqlCondition = sqlCondition + "ShipName = @ShipName,";
                    dynamicParams.Add("ShipName", UpdateData.ShipName);
                }
                if (UpdateData.ShipAddress != null)
                {
                    sqlCondition = sqlCondition + "ShipAddress = @ShipAddress,";
                    dynamicParams.Add("ShipAddress", UpdateData.ShipAddress);
                }
                if (UpdateData.ShipCity != null)
                {
                    sqlCondition = sqlCondition + "ShipCity = @ShipCity,";
                    dynamicParams.Add("ShipCity", UpdateData.ShipCity);
                }
                if (UpdateData.ShipRegion != null)
                {
                    sqlCondition = sqlCondition + "ShipRegion = @ShipRegion,";
                    dynamicParams.Add("ShipRegion", UpdateData.ShipRegion);
                }
                if (UpdateData.ShipPostalCode != null)
                {
                    sqlCondition = sqlCondition + "ShipPostalCode = @ShipPostalCode,";
                    dynamicParams.Add("ShipPostalCode", UpdateData.ShipPostalCode);
                }
                if (UpdateData.ShipCountry != null)
                {
                    sqlCondition = sqlCondition + "ShipCountry = @ShipCountry,";
                    dynamicParams.Add("ShipCountry", UpdateData.ShipCountry);
                }

                sqlCondition = sqlCondition.Remove(sqlCondition.LastIndexOf(","), 1);

                var SQL = conn.Execute(
                    $@"UPDATE Orders SET 
                    {sqlCondition}
                    WHERE OrderID = @OrderID"
                    , dynamicParams);


                conn.Close();


                bool result = true;

                return result;
            }else
            {
                return false;
            }
        }
        
    }
}
