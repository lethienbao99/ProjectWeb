using Mapster;
using Microsoft.Extensions.Logging;
using ProjectWeb.Bussiness.Services.Products;
using ProjectWeb.Common.Extensions.VnPay;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Common.UnitOfWorks;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.Categories;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Payments;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Services.Payments
{
    public class PaymentServices : Repository<Payment>, IPaymentServices
    {

        private readonly ProjectWebDBContext _context;
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly ILogger<PaymentServices> _logger;

        public PaymentServices(ProjectWebDBContext context, Lazy<IUnitOfWork> unitOfWork, ILogger<PaymentServices> logger) : base(context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResultMessage<PaymentLinkResponseModel>> CreatePayment(PaymentRequestModel request)
        {
            try
            {
                var payment = new Payment();
                payment = request.Adapt<Payment>();
                payment.ID = Guid.NewGuid();
                payment.Currency = "VND";
                _unitOfWork.Value.Payments.Insert(payment);

                var paymentSignature = new PaymentSignature();
                paymentSignature.PaymentID = payment.ID;
                paymentSignature.SignValue = request.SignValue;
                paymentSignature.IsValid = true;
                _unitOfWork.Value.PaymentSignatures.Insert(paymentSignature);

                int recordInsert = _unitOfWork.Value.Complete();

                var result = new PaymentLinkResponseModel();
                result.PaymentID = payment.ID;

                if (recordInsert > 0 && request.MerchantID != null)
                {
                    string paymentUrl = string.Empty;

                    var merchant = await _unitOfWork.Value.Merchants.GetByIDAsync(request.MerchantID.Value);
                    if(merchant.IsSuccessed)
                    {
                        switch (merchant.Object?.ShortName)
                        {
                            case "VNPay":
                                VnPayLibrary vnpay = new VnPayLibrary();
                                vnpay.AddRequestData("vnp_Version", merchant.Object.Version);
                                vnpay.AddRequestData("vnp_Command", "pay");
                                vnpay.AddRequestData("vnp_TmnCode", merchant.Object.Tmncode);
                                vnpay.AddRequestData("vnp_Amount", (request.RequiredAmount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

                               /* if (bankcode_Vnpayqr.Checked == true)
                                {
                                    vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
                                }
                                else if (bankcode_Vnbank.Checked == true)
                                {
                                    vnpay.AddRequestData("vnp_BankCode", "VNBANK");
                                }
                                else if (bankcode_Intcard.Checked == true)
                                {
                                    vnpay.AddRequestData("vnp_BankCode", "INTCARD");
                                }*/

                                vnpay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
                                vnpay.AddRequestData("vnp_CurrCode", "VND");
                                vnpay.AddRequestData("vnp_IpAddr", "13.160.92.202");
                                vnpay.AddRequestData("vnp_Locale", "vn");
                                vnpay.AddRequestData("vnp_OrderInfo", request.Content);
                                vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
                                vnpay.AddRequestData("vnp_ReturnUrl", merchant.Object.MerchantReturnUrl);
                                vnpay.AddRequestData("vnp_TxnRef", request.RefID.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
                                //Add Params of 2.1.0 Version
                                //Billing
                                 paymentUrl = vnpay.CreateRequestUrl(merchant.Object.MerchantPayLink, merchant.Object.SerectKey);
                                break;
                            
                            default:
                                break;
                        }
                    }
                    result.PaymentUrl = paymentUrl;
                }

                if(!string.IsNullOrEmpty(result.PaymentUrl))
                    return new ResultObjectSuccess<PaymentLinkResponseModel>(result);

                return new ResultObjectError<PaymentLinkResponseModel>("Can't create payment link!");
            }
            catch (Exception e)
            {
                return new ResultObjectError<PaymentLinkResponseModel>(e.Message);
            }
        }

        public Task GetLinkPayment(string paymentLink)
        {
            throw new NotImplementedException();
        }
    }
}
