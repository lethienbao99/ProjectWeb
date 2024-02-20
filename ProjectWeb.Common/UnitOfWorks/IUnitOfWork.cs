using ProjectWeb.Common.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IProductServices Products { get; }
        IStorageServices Images { get; }
        ICategoryServices Categories { get; }
        IOrderServices Orders { get; }
        IMessageServices Message { get; }
        IProductCategoriesServices ProductCategories { get; }
        IPaymentServices Payments { get; }
        IPaymentSignatureServices PaymentSignatures { get; }
        IMerchantServices Merchants { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}
