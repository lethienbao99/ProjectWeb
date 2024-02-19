using Mapster;
using Microsoft.Extensions.Logging;
using ProjectWeb.Bussiness.Services.Products;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Common.UnitOfWorks;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.Categories;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Payments;
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
                _unitOfWork.Value.Payments.Insert(payment);

                var paymentSignature = new PaymentSignature();
                paymentSignature.PaymentID = payment.ID;
                paymentSignature.SignValue = request.SignValue;
                paymentSignature.IsValid = true;
                _unitOfWork.Value.PaymentSignatures.Insert(paymentSignature);

                int recordInsert = _unitOfWork.Value.Complete();

                var result = new PaymentLinkResponseModel();
                result.PaymentID = payment.ID;

                if (recordInsert > 0)
                {
                    string paymentUrl = string.Empty;
                    switch ("VNPAY")
                    {
                        case "VNPAY":
                            var vnpayPayRequest = new VnPayPaymentRequestModel(
                                "2.1.0",
                                "APPZFC7N", 
                                DateTime.Now, 
                                "13.160.92.202" ?? string.Empty, 
                                request.RequiredAmount ?? 0, 
                                request.Currency ?? string.Empty,
                                "other", 
                                request.Content ?? string.Empty, 
                                "https://localhost:7277/payment/api/vnpay-return", 
                                result.PaymentID.Value.ToString() ?? string.Empty);

                            paymentUrl = vnpayPayRequest.GetLink("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", "YONPSVXYSUNSPVKIUOOOWXASIHLLYIFS");
                            break;
                        /*case "MOMO":
                            var momoOneTimePayRequest = new MomoOneTimePaymentRequest(momoConfig.PartnerCode,
                                outputIdParam!.Value?.ToString() ?? string.Empty, (long)request.RequiredAmount!, outputIdParam!.Value?.ToString() ?? string.Empty,
                                request.PaymentContent ?? string.Empty, momoConfig.ReturnUrl, momoConfig.IpnUrl, "captureWallet",
                                string.Empty);
                            momoOneTimePayRequest.MakeSignature(momoConfig.AccessKey, momoConfig.SecretKey);
                            (bool createMomoLinkResult, string? createMessage) = momoOneTimePayRequest.GetLink(momoConfig.PaymentUrl);
                            if (createMomoLinkResult)
                            {
                                paymentUrl = createMessage;
                            }
                            else
                            {
                                result.Message = createMessage;
                            }
                            break;
                        case "ZALOPAY":
                            var zalopayPayRequest = new CreateZalopayPayRequest(zaloPayConfig.AppId, zaloPayConfig.AppUser,
                                DateTime.Now.GetTimeStamp(), (long)request.RequiredAmount!, DateTime.Now.ToString("yymmdd") + "_" + outputIdParam!.Value?.ToString() ?? string.Empty,
                                "zalopayapp", request.PaymentContent ?? string.Empty);
                            zalopayPayRequest.MakeSignature(zaloPayConfig.Key1);
                            (bool createZaloPayLinkResult, string? createZaloPayMessage) = zalopayPayRequest.GetLink(zaloPayConfig.PaymentUrl);
                            if (createZaloPayLinkResult)
                            {
                                paymentUrl = createZaloPayMessage;
                            }
                            else
                            {
                                result.Message = createZaloPayMessage;
                            }
                            break;*/
                        default:
                            break;
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
