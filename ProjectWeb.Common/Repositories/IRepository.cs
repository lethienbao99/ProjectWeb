using ProjectWeb.Data.Entities;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetByID(Guid id);
        void Insert(T entity);
        void Update(T entity);
        void Update(Guid id);
        void DeleteByModel(T entity);
        string DeleteByID(Guid id);
        void DeleteNotSQLByModel(T entity);
        string DeleteNotSQLByID(Guid id);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        //Async
        Task<ResultMessage<IEnumerable<T>>> GetAllAsync();
        Task<ResultMessage<T>> GetByIDAsync(Guid id);
        Task<ResultMessage<int>> InsertAsync(T entity);
        Task<ResultMessage<int>> UpdateAsync(T entity);
        Task<ResultMessage<int>> UpdateAsync(Guid id);
        Task DeleteByModelAsync(T entity);
        Task<ResultMessage<bool>> DeleteByIDAsync(Guid id);
        Task DeleteNotSQLByModelAsync(T entity);
        Task<string> DeleteNotSQLByIDAsync(Guid id);



    }
}
