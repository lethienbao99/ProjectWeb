using ProjectWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetByID(Guid id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteNotSQL(T entity);
    }
}
