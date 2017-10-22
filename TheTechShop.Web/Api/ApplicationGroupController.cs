using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TheTechShop.Common.Exceptions;
using TheTechShop.Model.Models;
using TheTechShop.Service;
using TheTechShop.Web.App_Start;
using TheTechShop.Web.Infrastructure.Core;
using TheTechShop.Web.Models;
using TheTechShop.Web.Infrastructure.Extensions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TheTechShop.Web.Api
{
    [RoutePrefix("api/applicationGroup")]
    [Authorize]
    public class ApplicationGroupController : ApiControllerBase
    {
        private IApplicationGroupService _applicationGroupService;
        private IApplicationRoleService _applicationRoleService;
        private ApplicationUserManager _applicationUserManager;

        public ApplicationGroupController(IErrorService errorService, 
            IApplicationRoleService applicationRoleService,
            IApplicationGroupService applicationGroupService,
            ApplicationUserManager applicationUserManager) : base(errorService)
        {
            _applicationGroupService = applicationGroupService;
            _applicationRoleService = applicationRoleService;
            _applicationUserManager = applicationUserManager;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;
                var model = _applicationGroupService.GetAll(page, pageSize, out totalRow, filter);
                IEnumerable<ApplicationGroupViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(model);

                PaginationSet<ApplicationGroupViewModel> pagedSet = new PaginationSet<ApplicationGroupViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = modelVm
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _applicationGroupService.GetAll();
                IEnumerable<ApplicationGroupViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("detail/{id:int}")]
        [HttpGet]
        public HttpResponseMessage Detail(HttpRequestMessage request, int id)
        {
            if (id == 0)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " is required.");
            }
            ApplicationGroup applicationGroup = _applicationGroupService.GetDetail(id);
            var appGroupViewModel = Mapper.Map<ApplicationGroup, ApplicationGroupViewModel>(applicationGroup);
            if (applicationGroup == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "No group");
            }
            var listRole = _applicationRoleService.GetListRoleByGroupId(appGroupViewModel.ID);
            appGroupViewModel.Roles = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(listRole);
            return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
        }

        [Route("add")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, ApplicationGroupViewModel applicationGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppGroup = new ApplicationGroup();
                newAppGroup.Name = applicationGroupViewModel.Name;
                try
                {
                    var appGroup = _applicationGroupService.Add(newAppGroup);
                    _applicationGroupService.Save();

                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    foreach (var role in applicationGroupViewModel.Roles)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup()
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.Id
                        });
                    }
                    _applicationRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    _applicationRoleService.Save();

                    return request.CreateResponse(HttpStatusCode.OK, applicationGroupViewModel);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("update")]
        [HttpPost]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, ApplicationGroupViewModel applicationGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var appGroup = _applicationGroupService.GetDetail(applicationGroupViewModel.ID);
                try
                {
                    appGroup.UpdateApplicationGroup(applicationGroupViewModel);
                    _applicationGroupService.Update(appGroup);

                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    foreach (var role in applicationGroupViewModel.Roles)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup()
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.Id
                        });
                    }
                    _applicationRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    _applicationRoleService.Save();

                    //add role to user
                    var listRole = _applicationRoleService.GetListRoleByGroupId(appGroup.ID);
                    var listUserInGroup = _applicationGroupService.GetListUserByGroupId(appGroup.ID);
                    foreach (var user in listUserInGroup)
                    {
                        var listRoleName = listRole.Select(x => x.Name).ToArray();
                        foreach (var roleName in listRoleName)
                        {
                            await _applicationUserManager.RemoveFromRoleAsync(user.Id, roleName);
                            await _applicationUserManager.AddToRolesAsync(user.Id, roleName);
                        }
                    }
                    return request.CreateResponse(HttpStatusCode.OK, applicationGroupViewModel);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            var appGroup = _applicationGroupService.Delete(id);
            _applicationGroupService.Save();
            return request.CreateResponse(HttpStatusCode.OK, appGroup);
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedList)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listItem = new JavaScriptSerializer().Deserialize<List<int>>(checkedList);
                    foreach (var item in listItem)
                    {
                        _applicationGroupService.Delete(item);
                    }

                    _applicationGroupService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listItem.Count);
                }

                return response;
            });
        }
    }
}
