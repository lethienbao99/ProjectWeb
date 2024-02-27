using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Common.UnitOfWorks;
using ProjectWeb.Models.Categories;
using ProjectWeb.Models.Payments;
using System.Threading.Tasks;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PaymentsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public PaymentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestModel request)
        {
            var result = await _unitOfWork.Payments.CreatePayment(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> ReturnPayment(long? vnp_Amount, string vnp_BankCode, string vnp_BankTranNo, string vnp_CardType, string vnp_OrderInfo, string vnp_PayDate, string vnp_ResponseCode, string vnp_TmnCode, string vnp_TransactionNo, string vnp_TransactionStatus, string vnp_TxnRef, string vnp_SecureHashType, string vnp_SecureHash)
        {
            var result = await _unitOfWork.Payments.ReturnPayment(vnp_Amount, vnp_BankCode, vnp_BankTranNo, vnp_CardType, vnp_OrderInfo, vnp_PayDate, vnp_ResponseCode, vnp_TmnCode, vnp_TransactionNo, vnp_TransactionStatus, vnp_TxnRef, vnp_SecureHashType, vnp_SecureHash);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result.Message);
        }


    }
}
