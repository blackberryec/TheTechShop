using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTechShop.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private TheTechShopDbContext dbContext;

        public TheTechShopDbContext Init()
        {
            return dbContext ?? (dbContext = new TheTechShopDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
