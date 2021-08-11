using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.IServices
{
    public interface IOrderServices : IRepository<Order>
    {
        public Task<ResultMessage<bool>> CreateOrder(OrderViewModel request);
    }
}
