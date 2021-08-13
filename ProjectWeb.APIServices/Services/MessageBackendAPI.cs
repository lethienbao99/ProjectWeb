using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.APIServices.Services
{
    public class MessageBackendAPI : BaseBackendAPI, IMessageBackendAPI
    {
        public MessageBackendAPI(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {

        }
        public async Task<ResultMessage<List<MessageViewModel>>> GetMessageByProduct(Guid ProductID)
        {
            return await GetAndReturnAsync<List<MessageViewModel>>($"/api/Messages/{ProductID}", false);
        }

        public async Task<ResultMessage<bool>> PostMessage(MessageRequest request)
        {
            return await PostAndReturnAsync<bool, MessageRequest>("/api/Messages/", request, false);
        }
    }
}
