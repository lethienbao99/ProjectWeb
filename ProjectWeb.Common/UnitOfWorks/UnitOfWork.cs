using ProjectWeb.Common.IServices;
using ProjectWeb.Data.EntityFamework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProjectWebDBContext _context;

        //Add Interface Services this here.
        public IProductServices Products { get; }
        public IStorageServices Images { get; }

        public ICategoryServices Categories { get; }
        public IOrderServices Orders { get; }
        public IMessageServices Message { get; }

        public IProductCategoriesServices ProductCategories { get; }


        public UnitOfWork(
            ProjectWebDBContext context,
            IProductServices productServices,
            IStorageServices storageServices,
            ICategoryServices categoryServices,
            IOrderServices orderServices,
            IMessageServices messageServices,
            IProductCategoriesServices productCategoryServices
            )
        {
            //DBContext.
            _context = context;

            //Services.
            Products = productServices;
            Images = storageServices;
            Categories = categoryServices;
            Orders = orderServices;
            Message = messageServices;
            ProductCategories = productCategoryServices;
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
