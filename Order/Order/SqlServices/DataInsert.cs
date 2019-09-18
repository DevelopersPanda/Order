using Dapper;
using Order.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Order.SqlServices
{
    public class DataInsert
    {
        public int InsertOrderData(InsertOrders Insertdata)
        {
            if (Insertdata.ProductID != 0)
            {
                var dynamicParams = new DynamicParameters();//←動態參數

                SqlConnection conn = new SqlConnection("Data Source=howardorder.database.windows.net;Initial Catalog=OrderDatabase;Persist Security Info=True;User Id =howard;Password=Yihao1222");
                conn.Open();

                var queryOrderID = conn.Query<OrderDataVM>(
                    @"select max(OrderID) +1 as OrderID From Orders"
                    );

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
                dynamicParams.Add("ProductID", Insertdata.ProductID);
                
                //dynamicParams.Add("Quantity", Insertdata.Quantity);
                //dynamicParams.Add("Discount", Insertdata.Discount);

                //var queryUnitPrice = conn.Query<InsertProduct>(
                //    @"select Unitprice From [Products] where ProductID = @ProductID"
                //    , dynamicParams);

                //float EndUnitPrice = queryUnitPrice.First().UnitPrice;
                //EndUnitPrice = EndUnitPrice - (EndUnitPrice * Insertdata.Discount);

                //dynamicParams.Add("UnitPrice", EndUnitPrice);

                var SQL = conn.Execute(
                    @"SET IDENTITY_INSERT Orders ON
                    insert into Orders
                    (OrderID ,CustomerID ,EmployeeID ,OrderDate ,RequiredDate ,ShippedDate ,ShipVia ,Freight ,ShipName ,ShipAddress ,ShipCity ,ShipRegion ,ShipPostalCode ,ShipCountry)
                    Values
                    (@OrderID ,@CustomerID ,@EmployeeID ,@OrderDate ,@RequiredDate ,@ShippedDate ,@ShipVia ,@Freight ,@ShipName ,@ShipAddress ,@ShipCity ,@ShipRegion ,@ShipPostalCode ,@ShipCountry)
                    
                    SET IDENTITY_INSERT Orders Off"
                    , dynamicParams);

                conn.Close();

                return queryOrderID.First().OrderID;
            }
            else
            {
                return 0;
            }

        }
        public bool InsertOrderDetailData(InsertOrders Insertdata)
        {
            var dynamicParams = new DynamicParameters();//←動態參數
            SqlConnection conn = new SqlConnection("Data Source=howardorder.database.windows.net;Initial Catalog=OrderDatabase;Persist Security Info=True;User Id =howard;Password=Yihao1222");
            conn.Open();

            dynamicParams.Add("ProductID", Insertdata.ProductID);
            var queryUnitPrice = conn.Query<InsertProduct>(
                    @"select Unitprice From [Products] where ProductID = @ProductID"
                    , dynamicParams);

            float EndUnitPrice = queryUnitPrice.First().UnitPrice;
            EndUnitPrice = EndUnitPrice - (EndUnitPrice * Insertdata.Discount);

            dynamicParams.Add("OrderID", Insertdata.OrderID);
            dynamicParams.Add("UnitPrice", EndUnitPrice);
            dynamicParams.Add("Quantity", Insertdata.Quantity);
            dynamicParams.Add("Discount", Insertdata.Discount);

            var SQL = conn.Execute(
                    @"insert into [Order Details]
                    (OrderID ,ProductID ,UnitPrice ,Quantity ,Discount) 
                    Values
                    (@OrderID ,@ProductID ,@UnitPrice ,@Quantity ,@Discount)"
                    , dynamicParams);

            conn.Close();

            return true;
        }

    }
}
