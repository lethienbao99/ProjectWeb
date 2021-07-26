﻿using Microsoft.AspNetCore.Http;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.Products
{
    public class ProductModel : BaseModel
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Alias { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }

    public class ProductViewModel : BaseModel
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Alias { get; set; }
        public IFormFile ThumbnailImage { get; set; }


        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
    }

    public class ProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public List<Guid> CategoryIds { get; set; }
    }

    public class ProductByCategoryIdPagingRequest : PagingRequestBase
    {
        public Guid? CategoryId { get; set; }
    }
}
