using System;

namespace TheTechShop.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        TheTechShopDbContext Init();
    }
}