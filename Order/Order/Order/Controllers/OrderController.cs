using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Order.Models;
using Order.SqlServices;
using static Order.Core.OrderResult;

namespace Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrderModel()
        //{
        //    var Orderlist = await _Orders.Orders.ToListAsync();
        //    return Orderlist;
        //}

        [HttpGet("{OrderData}")]
        public OrderResults<IEnumerable<OrderDataVM>> OrderQuery([FromQuery]int OrderID)
        {
            var Query = new DataQuery();
            var result = Query.GetOrderData(OrderID);
            return result;
        }

        [HttpPost("{OrderInsert}")]
        [Consumes("application/json")]
        public string OrderInsert([FromQuery]InsertOrders Insertdata,[FromBody]InsertOrderDetails IOD)
        {
            for (int i = 0; i < IOD.ProductID.Count(); i++)
            {
                //required information
                if (IOD.ProductID[i] == null)
                    return "Please input ProductId";
                if (IOD.Quantity[i] == null)
                    return "Please input Quantity";
                if (IOD.Discount[i] == null)
                    return "Please input Discount";
            }
            //Insert Orders
            var Insert = new DataInsert();
            var InsertOrder = Insert.InsertOrderData(Insertdata);
            //Insert OrderDetails
            var result = Insert.InsertOrderDetailData(IOD,InsertOrder);
            return result;
        }

        [HttpPut("OrderUpdate")]
        public string OrderUpdate([FromQuery]updateOrders UpdateData,[FromBody]UpdateOrderDetails UpdateDetailData)
        {
            //required information
            if (UpdateData.OrderID == 0)
                return "Please input OrderID";
            string result,updateOrder,updateOrderDetial;
            var Update = new DataUpdate();
            //Determine if Order need to update
            if (UpdateData.CustomerID != null || UpdateData.CustomerID != null || UpdateData.EmployeeID != null || UpdateData.OrderDate != null || UpdateData.RequiredDate != null || UpdateData.ShipVia != null ||
                UpdateData.Freight != null || UpdateData.ShipName != null || UpdateData.ShipAddress != null || UpdateData.ShipCity != null || UpdateData.ShipRegion != null || UpdateData.ShipPostalCode != null || UpdateData.ShipCountry != null)
            {
                updateOrder = Update.UpdateOrderData(UpdateData);
            }
            else
            {
                updateOrder = "Order not Edit";
            }
            //Determine if Order Detail need to update & ProductID is required information
            if (UpdateDetailData.ProductID != null & (UpdateDetailData.Quantity != null || UpdateDetailData.UnitPrice != null || UpdateDetailData.Discount != null))
            {
                updateOrderDetial = Update.UpdateOrderDetailData(UpdateDetailData, UpdateData.OrderID);
            }
            else
            {
                updateOrderDetial = "Order Detail not Edit";
            }
            result = updateOrder + " & " + updateOrderDetial;
            return result;
        }

        [HttpPost("OrderDelete")]
        public string OrderDelete([FromQuery]int Orderid)
        {
            //required information
            if (Orderid == 0)
                return "Please Input OrderID";
            var Delete = new DataDelete();
            var result = Delete.DeleteOrderData(Orderid);

            return result;
        }


    }
}