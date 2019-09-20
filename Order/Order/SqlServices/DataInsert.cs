using Dapper;
using Order.Models;
using System.Data.SqlClient;
using System.Linq;

namespace Order.SqlServices
{
    public class DataInsert
    {
        public int InsertOrderData(InsertOrders Insertdata)
        {
            var dynamicParams = new DynamicParameters();//←動態參數

            SqlConnection conn = new SqlConnection("Data Source=howardorder.database.windows.net;Initial Catalog=OrderDatabase;Persist Security Info=True;User Id =howard;Password=Yihao1222");
            conn.Open();

            var queryOrderID = conn.Query<OrderDataVM>(
                @"select max(OrderID) +1 as OrderID From Orders"
                );
            conn.Close();
            dynamicParams.Add("OrderID", queryOrderID.First().OrderID);
            dynamicParams.Add("CustomerID", Insertdata.CustomerID);
            dynamicParams.Add("EmployeeID", Insertdata.EmployeeID);
            dynamicParams.Add("OrderDate", Insertdata.OrderDate);
            dynamicParams.Add("RequiredDate", Insertdata.RequiredDate);
            dynamicParams.Add("ShippedDate", Insertdata.ShippedDate);
            dynamicParams.Add("ShipVia", Insertdata.ShipVia);
            dynamicParams.Add("Freight", Insertdata.Freight);
            dynamicParams.Add("ShipName", Insertdata.ShipName);
            dynamicParams.Add("ShipAddress", Insertdata.ShipAddress);
            dynamicParams.Add("ShipCity", Insertdata.ShipCity);
            dynamicParams.Add("ShipRegion", Insertdata.ShipRegion);
            dynamicParams.Add("ShipPostalCode", Insertdata.ShipPostalCode);
            dynamicParams.Add("ShipCountry", Insertdata.ShipCountry);

            string SqlString = @"SET IDENTITY_INSERT Orders ON
                insert into Orders
                    (OrderID ,CustomerID ,EmployeeID ,OrderDate ,RequiredDate ,ShippedDate ,ShipVia ,Freight ,ShipName 
                    ,ShipAddress ,ShipCity ,ShipRegion ,ShipPostalCode ,ShipCountry)
                    Values
                    (@OrderID ,@CustomerID ,@EmployeeID ,@OrderDate ,@RequiredDate ,@ShippedDate ,@ShipVia ,@Freight ,@ShipName
                    ,@ShipAddress ,@ShipCity ,@ShipRegion ,@ShipPostalCode ,@ShipCountry)
                SET IDENTITY_INSERT Orders Off";

            var Insert = new SqlServices();
            string result = Insert.SqlInsert(SqlString,dynamicParams);
            return queryOrderID.First().OrderID;
        }
        public string InsertOrderDetailData(InsertOrderDetails Insertdata,int getOrderID)
        {
            var dynamicParams = new DynamicParameters();//←動態參數
            SqlConnection conn = new SqlConnection("Data Source=howardorder.database.windows.net;Initial Catalog=OrderDatabase;Persist Security Info=True;User Id =howard;Password=Yihao1222");
            conn.Open();
            var Insert = new SqlServices();
            string SqlString;
            string result = "";

            for(int i = 0; i < Insertdata.ProductID.Count(); i++) {
                //Insertdata.OrderID[i] = getOrderID;

                dynamicParams.Add("ProductID", Insertdata.ProductID[i]);

                var queryUnitPrice = conn.Query<InsertProduct>(
                        @"select Unitprice From [Products] where ProductID = @ProductID"
                        , dynamicParams);

                float? EndUnitPrice = queryUnitPrice.First().UnitPrice;
                EndUnitPrice = EndUnitPrice - (EndUnitPrice * Insertdata.Discount[i]);

                dynamicParams.Add("OrderID", getOrderID);
                dynamicParams.Add("UnitPrice", EndUnitPrice);
                dynamicParams.Add("Quantity", Insertdata.Quantity[i]);
                dynamicParams.Add("Discount", Insertdata.Discount[i]);

                SqlString = @"insert into [Order Details]
                            (OrderID ,ProductID ,UnitPrice ,Quantity ,Discount) 
                            Values
                            (@OrderID ,@ProductID ,@UnitPrice ,@Quantity ,@Discount)";
                result = Insert.SqlInsert(SqlString, dynamicParams);
            }
            conn.Close();
            return result;
        }
    }
}
