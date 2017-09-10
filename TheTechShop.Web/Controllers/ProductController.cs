﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheTechShop.Common;
using TheTechShop.Model.Models;
using TheTechShop.Service;
using TheTechShop.Web.Infrastructure.Core;
using TheTechShop.Web.Models;

namespace TheTechShop.Web.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;
        IProductCategoryService _productCategoryService;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService)
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
        }
        // GET: Product
        public ActionResult Detail(int productId)
        {
            return View();
        }

        public ActionResult Category(int id, int page = 1)
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productService.GetListProductByCategoryIdPaging(id, page, pageSize, out totalRow);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var category = _productCategoryService.GetById(id);
            ViewBag.Category = Mapper.Map<ProductCategory, ProductViewModel>(category);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }
    }
}