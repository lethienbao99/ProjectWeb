using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.EcommerceApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageBackendAPI _messageBackendAPI;
        private readonly IConfiguration _config;
        public MessageController(IMessageBackendAPI messageBackendAPI, IConfiguration config)
        {
            _config = config;
            _messageBackendAPI = messageBackendAPI;
        }

        public async Task<IActionResult> GetListReviewProduct(Guid ProductID)
        {
            var result = await _messageBackendAPI.GetMessageByProduct(ProductID);
            return Json(result.Object);
        }

  
        public async Task<IActionResult> PostMessage(string TitleText, string MessageText, Guid UserID, Guid ProductID)
        {
            var request = new MessageRequest()
            {
                TitleText = TitleText,
                MessageText = MessageText,
                UserID = UserID,
                ProductID = ProductID
            };
            var result = await _messageBackendAPI.PostMessage(request);
            return Json(result.Object);
        }
    }
}
