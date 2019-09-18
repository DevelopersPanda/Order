using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Models;
using Order.SqlServices;
using Dapper;
using static Order.Core.OrderResult;

namespace Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderContext _Orders;

        public OrderController(OrderContext context)
        {
            _Orders = context;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrderModel()
        //{
        //    var Orderlist = await _Orders.Orders.ToListAsync();
        //    return Orderlist;
        //}

        [HttpGet("{OrderData}")]
        public OrderResults<IEnumerable<OrderDataVM>> OrderQuery([FromQuery]int id)
        {
            //i = 10248;

            var Query = new DataQuery();
            var result = Query.GetOrderData(id);
            return result;

        }

        [HttpPost("{OrderInsert}")]
        public bool OrderInsert([FromQuery]InsertOrders Insertdata)
        {
            var Insert = new DataInsert();
            var InsertOrder = Insert.InsertOrderData(Insertdata);

            if(InsertOrder == 0)
            {
                return false;
            }
            else
            {
                Insertdata.OrderID = InsertOrder;
                var result = Insert.InsertOrderDetailData(Insertdata);
                return true;
            }
        }

        [HttpPut("OrderUpdate")]
        public bool OrderUpdate([FromQuery]updateOrders UpdateData)
        {
            var Update = new DataUpdate();
            var result = Update.UpdateOrderData(UpdateData);
            return result;
        }

        [HttpPost("OrderDelete")]
        public bool OrderDelete([FromQuery]int id)
        {
            var Delete = new DataDelete();
            var result = Delete.DeleteOrderData(id);

            return result;
        }


    }
}