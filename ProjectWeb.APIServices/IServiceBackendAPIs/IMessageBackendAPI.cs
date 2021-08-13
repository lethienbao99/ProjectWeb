using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.APIServices.IServiceBackendAPIs
{
    public interface IMessageBackendAPI
    {
        Task<ResultMessage<bool>> PostMessage(MessageRequest request);
        Task<ResultMessage<List<MessageViewModel>>> GetMessageByProduct(Guid ProductID);
    }
}
