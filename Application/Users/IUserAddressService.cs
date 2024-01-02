using Application.Interfaces.Contexts;
using AutoMapper;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users
{
    public interface IUserAddressService
    {
        List<UserAddressDto> GetAddress(string UserId);
        void AddnewAddress(AddUserAddressDto address);
    }

    public class UserAddressService : IUserAddressService
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;

        public UserAddressService(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void AddnewAddress(AddUserAddressDto address)
        {
            var data = mapper.Map<UserAddress>(address);
            context.UserAddresses.Add(data);
            context.SaveChanges();
        }

        public List<UserAddressDto> GetAddress(string UserId)
        {
            var address = context.UserAddresses.Where(p => p.UserId == UserId);
            var data = mapper.Map<List<UserAddressDto>>(address);
            return data;
        }
    }


    public class UserAddressDto
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PostalAddress { get; set; }
        public string ReciverName { get; set; }
    }

    public class AddUserAddressDto
    {
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PostalAddress { get; set; }
        public string ReciverName { get; private set; }
        public string UserId { get; set; }
    }
}
