using ProjectWeb.Bussiness.Services.Commons;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Services.Orders
{
    public class OrderServices: Repository<Order>, IOrderServices
    {
        private readonly ProjectWebDBContext _context;
        public OrderServices(ProjectWebDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ResultMessage<bool>> CreateOrder(OrderViewModel request)
        {
            double TotalPriceFinal = 0;
            var user = _context.Users.FirstOrDefault(x => x.Id == request.UserID);
            var userinfo = _context.UserInformations.FirstOrDefault(x => x.ID == user.UserInfomationID);

            var Order = new Order()
            {
                ID = Guid.NewGuid(),
                UserID = request.UserID,
                ShipEmail = request.ShipEmail,
                ShipAddress = request.ShipAddress,
                ShipName = "Đơn hàng của " + user.UserName,
                ShipNumberPhone = userinfo.PhoneNumber,
                Status = "E_WAITING",
                DateOrderd = DateTime.Now,
                DateCreated = DateTime.Now
            };
            _context.Orders.Add(Order);
            if(request.ListItems.Count > 0)
            {
                foreach (var item in request.ListItems)
                {
                    TotalPriceFinal += item.Quantity * item.Price;
                    var orderDetail = new OrderDetail()
                    {
                        ID = Guid.NewGuid(),
                        OrderID = Order.ID,
                        ProductID = item.ProductID,
                        Quatity = item.Quantity,
                        TotalPrice = item.Quantity * item.Price,
                        DateCreated = DateTime.Now
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
            }
            Order.TotalPrice = TotalPriceFinal;

            var SendMailService = new SendMailServices();

            await SendMailService.SendMailGoogleSmtp("mailgui@mail.com", "mailnhan@mail.com", "Chủ đề", "Nội dung",
                                              "yourgmail@gmail.com", "yourgmailpassword");

            await _context.SaveChangesAsync();
            return new ResultObjectSuccess<bool>(true);
        }
    }
}
