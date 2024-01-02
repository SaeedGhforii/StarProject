using Application.Catalogs.CatalogItems.UriComposer;
using Application.Discounts;
using Application.Exceptions;
using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders
{
    public interface IOrderService
    {
        int CreateOrder(int BasketId, int UserAddressId, PaymentMethod paymentMethod);
    }

    public class OrderService : IOrderService
    {

        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        private readonly IUriComposerService uriComposerService;
        private readonly IDiscountHistoryService discountHistoryService;

        public OrderService(IDataBaseContext context
            , IMapper mapper
            , IUriComposerService uriComposerService
            , IDiscountHistoryService discountHistoryService)
        {
            this.context = context;
            this.mapper = mapper;
            this.uriComposerService = uriComposerService;
            this.discountHistoryService = discountHistoryService;
        }

        public int CreateOrder(int BasketId, int UserAddressId, PaymentMethod paymentMethod)
        {
  
  
            var basket = context.Baskets
                         .Include(p => p.Items)
                         .Include(p => p.AppliedDiscount)
                         .SingleOrDefault(p => p.Id == BasketId);

            if (basket == null)
                throw new NotFoundException(nameof(basket), BasketId);


            int[] Ids = basket.Items.Select(p => p.CatalogItemId).ToArray();
            var catalogItems = context.CatalogItems
                .Include(p => p.CatalogItemImages)
                .Where(p => Ids.Contains(p.Id));


            var orderItems = basket.Items.Select(basketItem =>
           {
               var catalogItem = catalogItems.First(c => c.Id == basketItem.CatalogItemId);

               var orderitem = new OrderItem(catalogItem.Id,
                   catalogItem.Name,
                   uriComposerService
                   .ComposeImageUri
                   (catalogItem?.CatalogItemImages?.FirstOrDefault()?.Src ?? ""),
                   catalogItem.Price, basketItem.Quantity);
               return orderitem;

           }).ToList();

            var userAddress = context.UserAddresses.SingleOrDefault(p => p.Id == UserAddressId);
            var address = mapper.Map<Address>(userAddress);
            var order = new Order(basket.BuyerId, address, orderItems, paymentMethod, basket.AppliedDiscount);
            context.Orders.Add(order);
            context.Baskets.Remove(basket);
            context.SaveChanges();

            if (basket.AppliedDiscount != null)
            {
                discountHistoryService.InsertDiscountUsageHistory(basket.Id, order.Id);
            }
            return order.Id;

        }
    }
}
