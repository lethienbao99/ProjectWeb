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
        private readonly ISendMailServices _sendMailServices;
        public OrderServices(ProjectWebDBContext context, ISendMailServices sendMailServices) : base(context)
        {
            _context = context;
            _sendMailServices = sendMailServices;
        }

        public async Task<ResultMessage<bool>> CreateOrder(OrderViewModel request)
        {
            double TotalPriceFinal = 0;

            var user = _context.Users.FirstOrDefault(x => x.Id == request.UserID);
            if(user == null)
                return new ResultObjectError<bool>("Fail");

            var userinfo = _context.UserInformations.FirstOrDefault(x => x.ID == user.UserInfomationID);
            if(userinfo == null)
                return new ResultObjectError<bool>("Fail");

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
            var result = await _context.SaveChangesAsync();
            if(result > 0)
            {
                //Tạo đơn hàng thành công thì gửi mail.
                var mailBody = "<h1> Xin chào, " + userinfo.FirstName + " " + userinfo.LastName + "</h1> <br/> Đơn hàng của bản đang được xử lý!! <br/> Xin cảm ơn!!";
                var isSendMail = await _sendMailServices.SendMailGoogleSmtp(request.ShipEmail, "Xác nhân đơn đặt hàng", mailBody);
                if(isSendMail.Object == true)
                    return new ResultObjectSuccess<bool>(true);
                else
                    return new ResultObjectError<bool>(isSendMail.Message);
            }
            return new ResultObjectSuccess<bool>(true);
        }
    }
}
