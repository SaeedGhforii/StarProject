using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CacheHelpers
{
    public static class CacheHelper
    {
        public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(60);
        private static readonly string _itemsKeyTemplate = "items-{0}-{1}-{2}";

 
        public static string GenerateCatalogItemCacheKey(int pageIndex, int itemsPage, int? typeId)
        {
            return string.Format(_itemsKeyTemplate, pageIndex, itemsPage, typeId);
        }
        public static string GenerateHomePageCacheKey()
        {
            return "HomePage";
        }


    }
}
