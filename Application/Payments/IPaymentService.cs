using Application.Interfaces.Contexts;
using Domain.Order;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Payments
{
    public interface IPaymentService
    {
        PaymentOfOrderDto PayForOrder(int OrderId);
        PaymentDto GetPayment(Guid Id);
        bool VerifyPayment(Guid Id, string Authority, long RefId);

    }

    public class PaymentService : IPaymentService
    {
        private readonly IDataBaseContext context;
        private readonly IIdentityDatabaseContext identityContext;
        public PaymentService(IDataBaseContext context, IIdentityDatabaseContext identityDatabaseContext)
        {
            this.context = context;
            identityContext = identityDatabaseContext;
        }

        public PaymentDto GetPayment(Guid Id)
        {
            var payment = context.Payments
                 .Include(p => p.Order)
                 .ThenInclude(p => p.OrderItems)
                 .Include(p=> p.Order)
                 .ThenInclude(p=> p.AppliedDiscount)
                 .SingleOrDefault(p => p.Id == Id);

            var user = identityContext.Users.SingleOrDefault(p => p.Id == payment.Order.UserId);

            string description = $"پرداخت سفارش شماره {payment.OrderId} " + Environment.NewLine;
            description += "محصولات" + Environment.NewLine;
            foreach (var item in payment.Order.OrderItems.Select(p => p.ProductName))
            {
                description += $" -{item}";
            }

            PaymentDto paymentDto = new PaymentDto
            {
                Amount = payment.Order.TotalPrice(),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Userid = user.Id,
                Id = payment.Id,
                Description = description,
            };
            return paymentDto;
        }

        public PaymentOfOrderDto PayForOrder(int OrderId)
        {
            var order = context.Orders
                    .Include(p => p.OrderItems)
                    .Include(p=>p.AppliedDiscount)
                    .SingleOrDefault(p => p.Id == OrderId);
            if (order == null)
                throw new Exception("");
            var payment = context.Payments.SingleOrDefault(p => p.OrderId == order.Id);

            if (payment == null)
            {
                payment = new Payment(order.TotalPrice(), order.Id);
                context.Payments.Add(payment);
                context.SaveChanges();
            }

            return new PaymentOfOrderDto()
            {
                Amount = payment.Amount,
                PaymentId = payment.Id,
                PaymentMethod = order.PaymentMethod,
            };


        }

        public bool VerifyPayment(Guid Id, string Authority, long RefId)
        {
            var payment = context.Payments
       .Include(p => p.Order)
       .SingleOrDefault(p => p.Id == Id);

            if (payment == null)
                throw new Exception("payment not found");

            payment.Order.PaymentDone();
            payment.PaymentIsDone(Authority, RefId);

            context.SaveChanges();
            return true;
        }
    }



    public class PaymentOfOrderDto
    {
        public Guid PaymentId { get; set; }
        public int Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }


    public class PaymentDto
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public int Amount { get; set; }
        public string Userid { get; set; }
    }
}
