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
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public PaymentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestModel request)
        {
            var result = await _unitOfWork.Payments.CreatePayment(request);
            if (result.IsSuccessed)
                return Ok(result);
            return BadRequest(result.Message);
        }
    }
}
