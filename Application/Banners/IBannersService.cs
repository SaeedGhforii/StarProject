using Application.Interfaces.Contexts;
using Domain.Banners;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Banners
{
    public interface IBannersService
    {
        void AddBanner(BannerDto banner);
        List<BannerDto> GetBanners();
    }

    public class BannersService : IBannersService
    {
        private readonly IDataBaseContext context;

        public BannersService(IDataBaseContext context)
        {
            this.context = context;
        }

        public void AddBanner(BannerDto banner)
        {
            context.Banners.Add(new Banner
            {
                Image = banner.Image,
                IsActive = banner.IsActive,
                Link = banner.Link,
                Name = banner.Name,
                Position = banner.Position,
                Priority = banner.Priority,
            });
            context.SaveChanges();
        }

        public List<BannerDto> GetBanners()
        {
            var banners = context.Banners
                .Select(p => new BannerDto
                {
                    Image = p.Image,
                    IsActive = p.IsActive,
                    Link = p.Link,
                    Name = p.Link,
                    Position = p.Position,
                    Priority = p.Priority,
                }).ToList();
            return banners;
        }
    }


    public class BannerDto
    {
        [Display(Name = "نام بنر")]
        public string Name { get; set; }
        [Display(Name = "تصویر بنر")]
        public string Image { get; set; }
        [Display(Name = "لینک")]
        public string Link { get; set; }
        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
        [Display(Name = "موقعیت نمایش")]
        public Position Position { get; set; }
        [Display(Name = "ترتیب نمایش")]
        public int Priority { get; set; }
    }
}
