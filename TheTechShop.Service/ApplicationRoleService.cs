using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTechShop.Common.Exceptions;
using TheTechShop.Data.Infrastructure;
using TheTechShop.Data.Repositories;
using TheTechShop.Model.Models;

namespace TheTechShop.Service
{
    public interface IApplicationRoleService
    {
        ApplicationRole GetDetail(string id);

        IEnumerable<ApplicationRole> GetAll(int page, int pageSize, out int totalRow, string filter);

        IEnumerable<ApplicationRole> GetAll();

        ApplicationRole Add(ApplicationRole appRole);

        void Update(ApplicationRole AppRole);

        void Delete(string id);

        //Add roles to a sepcify group
        bool AddRolesToGroup(IEnumerable<ApplicationRoleGroup> roleGroups, int groupId);

        //Get list role by group id
        IEnumerable<ApplicationRole> GetListRoleByGroupId(int groupId);

        void Save();
    }
    public class ApplicationRoleService : IApplicationRoleService
    {
        private IApplicationRoleRepository _applicationRoleRepository;
        private IApplicationRoleGroupRepository _applicationRoleGroupRepository;
        private IUnitOfWork _unitOfWork;

        public ApplicationRoleService(IUnitOfWork unitOfWork,
            IApplicationRoleGroupRepository applicationRoleGroupRepository, IApplicationRoleRepository applicationRoleRepository)
        {
            this._applicationRoleGroupRepository = applicationRoleGroupRepository;
            this._applicationRoleRepository = applicationRoleRepository;
            this._unitOfWork = unitOfWork;
        }

        public ApplicationRole Add(ApplicationRole appRole)
        {
            if (_applicationRoleRepository.CheckContains(x => x.Description == appRole.Description))
                throw new NameDuplicatedException("Tên không được trùng");
            {
                return _applicationRoleRepository.Add(appRole);
            }
        }

        public bool AddRolesToGroup(IEnumerable<ApplicationRoleGroup> roleGroups, int groupId)
        {
            _applicationRoleGroupRepository.DeleteMulti(x => x.GroupId == groupId);
            foreach (var roleGroup in roleGroups)
            {
                _applicationRoleGroupRepository.Add(roleGroup);
            }
            return true;
        }

        public void Delete(string id)
        {
            _applicationRoleRepository.DeleteMulti(x => x.Id == id);
        }

        public IEnumerable<ApplicationRole> GetAll(int page, int pageSize, out int totalRow, string filter = null)
        {
            var query = _applicationRoleRepository.GetAll();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Description.Contains(filter));
            }
            totalRow = query.Count();
            return query.OrderBy(x => x.Description).Skip(page * pageSize).Take(pageSize);
        }

        public IEnumerable<ApplicationRole> GetAll()
        {
            return _applicationRoleRepository.GetAll();
        }

        public ApplicationRole GetDetail(string id)
        {
            return _applicationRoleRepository.GetSingleByCondition(x => x.Id == id);
        }

        public IEnumerable<ApplicationRole> GetListRoleByGroupId(int groupId)
        {
            return _applicationRoleRepository.GetListRoleByGroup(groupId);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ApplicationRole AppRole)
        {
            if (_applicationRoleRepository.CheckContains(x=> x.Description == AppRole.Description && x.Id != AppRole.Id))
                throw new NameDuplicatedException("Tên không được trùng");
            {
                _applicationRoleRepository.Update(AppRole);
            }
        }
    }
}
