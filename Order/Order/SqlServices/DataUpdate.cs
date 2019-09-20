using Dapper;
using Order.Models;
using System.Data.SqlClient;
using System.Linq;

namespace Order.SqlServices
{
    public class DataUpdate
    {
        public string UpdateOrderData(updateOrders UpdateData)
        {
            var dynamicParams = new DynamicParameters();//←動態參數
            dynamicParams.Add("OrderID", UpdateData.OrderID);
            var sqlCondition = @"";
            string SqlString, result;
            var Update = new SqlServices();

            SqlConnection conn = new SqlConnection("Data Source=howardorder.database.windows.net;Initial Catalog=OrderDatabase;Persist Security Info=True;User Id =howard;Password=Yihao1222");
            conn.Open();
            //Determine if HasValue
            if (UpdateData.EmployeeID.HasValue)
            {
                sqlCondition = sqlCondition + " EmployeeID = @EmployeeID,";
                dynamicParams.Add("EmployeeID", UpdateData.EmployeeID);
            }
            if(UpdateData.CustomerID != null)
            {
                dynamicParams.Add("CustomerID", UpdateData.CustomerID);
                var queryCustomer = conn.Query<updateOrders>("select CustomerID From Customers where CustomerID = @CustomerID", dynamicParams);
                conn.Close();
                if (queryCustomer.Count() != 0)
                {
                    sqlCondition = sqlCondition + "CustomerID = @CustomerID,";
                }else
                {
                    conn.Close();
                    return "No such CustomerID";
                }
            }
            if (UpdateData.RequiredDate.HasValue)
            {
                sqlCondition = sqlCondition + "RequiredDate = @RequiredDate,";
                dynamicParams.Add("RequiredDate", UpdateData.RequiredDate);
            }
            if (UpdateData.ShippedDate.HasValue)
            {
                sqlCondition = sqlCondition + "ShippedDate = @ShippedDate,";
                dynamicParams.Add("ShippedDate", UpdateData.ShippedDate);
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

            SqlString = $@"UPDATE Orders SET 
                {sqlCondition}
                WHERE OrderID = @OrderID";
            result = Update.SqlUpdate(SqlString, dynamicParams);

            if(result.Equals("Update Success"))
                result = "Edit Order - Success";
            
            return result;
        }
        public string UpdateOrderDetailData(UpdateOrderDetails UOD,int getOrderID)
        {
            var dynamicParams = new DynamicParameters();//←動態參數
            var sqlCondition = @"";
            string SqlString, result;
            var Update = new SqlServices();

            SqlConnection conn = new SqlConnection("Data Source=howardorder.database.windows.net;Initial Catalog=OrderDatabase;Persist Security Info=True;User Id =howard;Password=Yihao1222");
            conn.Open();
            
            for(int i = 0; i < UOD.ProductID.Count(); i++)
            {
                sqlCondition = @"";
                dynamicParams.Add("OrderID", getOrderID);
                dynamicParams.Add("ProductID", UOD.ProductID[i]);
                if (UOD.UnitPrice[i] != null)
                {
                    dynamicParams.Add("UnitPrice", UOD.UnitPrice[i]);
                }
                else
                {
                    var queryUnitPrice = conn.Query<queryPrice>(
                        @"Select a.Unitprice,b.Discount From [Products] a Join [Order Details] b on a.ProductID = b.ProductID Where b.OrderID = OrderID And b.ProductID = @ProductID Group by a.Unitprice,b.Discount"
                        , dynamicParams);
                    float? EndUnitPrice = queryUnitPrice.First().UnitPrice;
                    //計算金額
                    if (UOD.Discount[i] != null)
                        EndUnitPrice = EndUnitPrice - (EndUnitPrice * UOD.Discount[i]);
                    else
                        EndUnitPrice =  queryUnitPrice.First().Discount * EndUnitPrice;
                    dynamicParams.Add("UnitPrice", EndUnitPrice);
                }
                sqlCondition = sqlCondition + "UnitPrice = @UnitPrice,";

                if (UOD.Quantity[i] != null)
                {
                    sqlCondition = sqlCondition + "Quantity = @Quantity,";
                    dynamicParams.Add("Quantity", UOD.Quantity[i]);
                }
                if (UOD.Discount[i] != null)
                {
                    sqlCondition = sqlCondition + "Discount = @Discount,";
                    dynamicParams.Add("Discount", UOD.Discount[i]);
                }
                sqlCondition = sqlCondition.Remove(sqlCondition.LastIndexOf(","), 1);

                var queryCustomer = conn.Query<updateOrders>(@"select PD.ProductID From Products PD Join[Order Details] OD on PD.ProductID = OD.ProductID 
                                                            where OrderID = @OrderID And PD.ProductID = @ProductID", dynamicParams);
                conn.Close();
                if (queryCustomer.Count() != 0)
                {
                    SqlString = $@"UPDATE [Order Details] SET 
                                    {sqlCondition}
                                    WHERE OrderID = @OrderID
                                    And ProductID = @ProductID";
                    result = Update.SqlUpdate(SqlString, dynamicParams);
                }
                else
                {
                    return "No such ProductID";
                }
            }
            return "Edit Order Detail - Success";
        }
    }
}
