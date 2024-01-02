using Application.Interfaces.Contexts;
using Domain.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.CustomerOrdersServices
{
    public interface ICustomerOrdersService
    {
        List<MyOrderDto> GetMyOrder(string userId);
    }

    public class CustomerOrdersService : ICustomerOrdersService
    {

        private readonly IDataBaseContext context;

        public CustomerOrdersService(IDataBaseContext context)
        {
            this.context = context;
        }

        public List<MyOrderDto> GetMyOrder(string userId)
        {

 
            var orders = context.Orders
                .Include(p => p.OrderItems)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.Id)
                .Select(p => new MyOrderDto
                {
                    Id = p.Id,
                    Date = context.Entry(p).Property("InsertTime").CurrentValue.ToString(),
                    OrderStatus = p.OrderStatus,
                    PaymentStatus = p.PaymentStatus,
                    Price = p.TotalPrice()

                }).ToList();
            return orders;
        }
    }

    public class MyOrderDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int Price { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }

}
