using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Models;
using Dapper;

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

        public IEnumerable<OrderModel> OrderQuery([FromQuery]int id)
        {
            //i = 10248;

            var dynamicParams = new DynamicParameters();//←動態參數
            dynamicParams.Add("OrderID", id);


            SqlConnection conn = new SqlConnection("Data Source=howardorder.database.windows.net;Initial Catalog=OrderDatabase;Persist Security Info=True;User Id =howard;Password=Yihao1222");
            conn.Open();

            string strSql = @"Select * from Orders where [OrderID] = @OrderID";
            var result = conn.Query<OrderModel>(strSql,dynamicParams);

            conn.Close();

            return result;
        }
    }
}