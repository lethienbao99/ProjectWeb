using Microsoft.EntityFrameworkCore;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {

        private readonly ProjectWebDBContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public Repository(ProjectWebDBContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public void DeleteNotSQL(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.IsDelete = true;
            entity.DateDeleted = DateTime.Now;
            context.SaveChanges();
        }


        public IEnumerable<T> GetAll()
        {
            return entities.Where(x => x.IsDelete == null).AsEnumerable();
        }

        public T GetByID(Guid id)
        {
            if(id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("ID is null or empty");
            }
            else
            {
                return entities.FirstOrDefault(s => s.ID == id && s.IsDelete == null);
            }
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.DateCreated = DateTime.Now;
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.DateUpdated = DateTime.Now;
            context.SaveChanges();
        }
    }
}
