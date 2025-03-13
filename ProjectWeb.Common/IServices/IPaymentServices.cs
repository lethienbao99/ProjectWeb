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

        Task<ResultMessage<string>> ReturnPayment(long? vnp_Amount, string vnp_BankCode, string vnp_BankTranNo, string vnp_CardType, string vnp_OrderInfo, string vnp_PayDate, string vnp_ResponseCode, string vnp_TmnCode, string vnp_TransactionNo, string vnp_TransactionStatus, string vnp_TxnRef, string vnp_SecureHashType, string vnp_SecureHash);

    }
}
