using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.IServices
{
    public interface IPaymentServices : IRepository<Payment>
    {
        Task<ResultMessage<PaymentLinkResponseModel>> CreatePayment(PaymentRequestModel request);
    }
}
