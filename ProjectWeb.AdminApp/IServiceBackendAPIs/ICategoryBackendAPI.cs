using ProjectWeb.Models;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.IServiceBackendAPIs
{
    public interface ICategoryBackendAPI
    {
        Task<ResultMessage<List<CategoryModel>>> GetAll();
    }
}
