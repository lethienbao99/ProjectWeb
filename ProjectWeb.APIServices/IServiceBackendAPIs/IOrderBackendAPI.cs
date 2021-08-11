using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.APIServices.IServiceBackendAPIs
{
    public interface IOrderBackendAPI
    {
        Task<ResultMessage<bool>> CreateOrder(OrderViewModel request);
    }
}
