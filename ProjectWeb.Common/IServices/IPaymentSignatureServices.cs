using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.IServices
{
    public interface IPaymentSignatureServices : IRepository<PaymentSignature>
    {
    }
}
