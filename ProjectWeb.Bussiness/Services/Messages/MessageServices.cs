using Microsoft.EntityFrameworkCore;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Services.Messages
{
    public class MessageServices : Repository<Message>, IMessageServices
    {
        private readonly ProjectWebDBContext _context;
        public MessageServices(ProjectWebDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ResultMessage<List<MessageViewModel>>> GetMessageByProduct(Guid ProductID)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.ID == ProductID);
            if(product != null)
            {
                var listMessage = from m in _context.Messages
                                  join p in _context.Products on m.ProductID equals p.ID
                                  join u in _context.Users on m.UserID equals u.Id
                                  where m.ProductID == ProductID
                                  select new { m, u};

                if(listMessage != null)
                {
                    var data = await listMessage.Take(10).OrderByDescending(x => x.m.DateCreated)
                     .Select(x => new MessageViewModel()
                     {
                        Username = x.u.UserName,
                        TitleText = x.m.TitleText,
                        MessageText = x.m.MessageText,
                        DateCreated = x.m.DateCreated
                     }).ToListAsync();
                    if (data != null)
                        return new ResultObjectSuccess<List<MessageViewModel>>(data);
                }
                return new ResultObjectError<List<MessageViewModel>>("Fail");
            }
            return new ResultObjectError<List<MessageViewModel>>("Fail");
        }

        public async Task<ResultMessage<bool>> PostMessage(MessageRequest request)
        {
            
            if (request.UserID != Guid.Empty && request.ProductID != Guid.Empty)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserID);
                var product = await _context.Products.FirstOrDefaultAsync(x => x.ID == request.ProductID);
                if(user != null && product != null)
                {
                    var message = new Message()
                    {
                        UserID = user.Id,
                        MessageText = request.MessageText,
                        TitleText = request.TitleText,
                        ProductID = product.ID,
                        DateCreated = DateTime.Now
                    };
                    _context.Messages.Add(message);
                    await _context.SaveChangesAsync();
                    return new ResultObjectSuccess<bool>(true);
                }
            }
            return new ResultObjectError<bool>("Fail");
        }
    }
}
