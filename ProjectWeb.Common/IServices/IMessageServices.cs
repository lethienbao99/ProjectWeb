using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.IServices
{
    public interface IMessageServices :  IRepository<Message>
    {
        Task<ResultMessage<bool>> PostMessage(MessageRequest request);
        Task<ResultMessage<List<MessageViewModel>>> GetMessageByProduct(Guid ProductID);
    }
}
