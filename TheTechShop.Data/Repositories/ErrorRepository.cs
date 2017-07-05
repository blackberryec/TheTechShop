using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTechShop.Data.Infrastructure;
using TheTechShop.Model.Models;

namespace TheTechShop.Data.Repositories
{
    public interface IErrorRepository : IRepository<Error>
    {

    }
    public class ErrorRepository : RepositoryBase<Error>, IErrorRepository
    {
        public ErrorRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
