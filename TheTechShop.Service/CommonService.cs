using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTechShop.Common;
using TheTechShop.Data.Infrastructure;
using TheTechShop.Data.Repositories;
using TheTechShop.Model.Models;

namespace TheTechShop.Service
{
    public interface ICommonService
    {
        Footer GetFooter();
    }
    public class CommonService : ICommonService
    {
        IFooterRepository _footerRepository;
        IUnitOfWork _unitOfWork;
        public CommonService(IFooterRepository footerRepository, IUnitOfWork unitOfWork)
        {
            _footerRepository = footerRepository;
            _unitOfWork = unitOfWork;
        }
        public Footer GetFooter()
        {
            return _footerRepository.GetSingleByCondition(x => x.ID == CommonConstants.DefaultFooterId);
        }
    }
}
