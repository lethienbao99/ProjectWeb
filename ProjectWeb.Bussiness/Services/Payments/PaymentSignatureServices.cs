using Microsoft.Extensions.Logging;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Common.UnitOfWorks;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Services.Payments
{
    public class PaymentSignatureServices : Repository<PaymentSignature>, IPaymentSignatureServices
    {
        private readonly ProjectWebDBContext _context;
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly ILogger<PaymentServices> _logger;

        public PaymentSignatureServices(ProjectWebDBContext context, Lazy<IUnitOfWork> unitOfWork, ILogger<PaymentServices> logger) : base(context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
    }
}
