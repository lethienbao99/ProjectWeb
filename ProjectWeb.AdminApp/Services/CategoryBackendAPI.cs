﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ProjectWeb.AdminApp.IServiceBackendAPIs;
using ProjectWeb.Models;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.Services
{
    public class CategoryBackendAPI : BaseBackendAPI, ICategoryBackendAPI
    {
        public CategoryBackendAPI(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {

        }
        public async Task<ResultMessage<List<CategoryModel>>> GetAll()
        {
            return await GetAndReturnAsync<List<CategoryModel>>("/api/Categories/");
        }
    }
}
