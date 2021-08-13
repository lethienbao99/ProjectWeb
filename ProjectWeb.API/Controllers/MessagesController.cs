using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Common.UnitOfWorks;
using ProjectWeb.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public MessagesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> PostMessages([FromBody] MessageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _unitOfWork.Message.PostMessage(request);
            if (result.Object == false)
                return BadRequest("Create Fail!!!");

            return Ok(result);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetMessagesByProductId(Guid productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _unitOfWork.Message.GetMessageByProduct(productId);
            if (result.Object == null)
                return BadRequest("Create Fail!!!");

            return Ok(result);
        }


    }
}
