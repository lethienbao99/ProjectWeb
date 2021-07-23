using Microsoft.EntityFrameworkCore;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {

        private readonly ProjectWebDBContext _context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public Repository(ProjectWebDBContext context)
        {
            _context = context;
            entities = context.Set<T>();
        }
        public void DeleteByModel(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _context.SaveChanges();
        }


        public string DeleteByID(Guid id)
        {
            var mess = "Fail";
            var result = entities.Find(id);
            if(result != null)
            {
                entities.Remove(result);
                _context.SaveChanges();
                mess = "Success";
                return mess;
            }
            else
                return mess;
        }


        public void DeleteNotSQLByModel(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.IsDelete = true;
            entity.DateDeleted = DateTime.Now;
            _context.SaveChanges();
        }


        public IEnumerable<T> GetAll()
        {
            return entities.Where(x => x.IsDelete == null).AsEnumerable();
        }

        public T GetByID(Guid id)
        {
            if(id == Guid.Empty)
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
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.DateUpdated = DateTime.Now;
            _context.SaveChanges();
        }

        public string DeleteNotSQLByID(Guid id)
        {
            var mess = "Fail";
            var result = entities.FirstOrDefault(x => x.ID == id && x.IsDelete == null);
            if (result != null)
            {
                result.IsDelete = true;
                result.DateDeleted = DateTime.Now;
                _context.SaveChanges();
                mess = "Success";
                return mess;
            }
            else
                return mess;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIDAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("ID is null or empty");
            }
            else
            {
                return await entities.FirstOrDefaultAsync(s => s.ID == id && s.IsDelete == null);
            }
        }

        public async Task InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.DateCreated = DateTime.Now;
            await entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.DateUpdated = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByModelAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<string> DeleteByIDAsync(Guid id)
        {
            var mess = "Fail";
            var result = entities.Find(id);
            if (result != null)
            {
                entities.Remove(result);
                await _context.SaveChangesAsync();
                mess = "Success";
                return mess;
            }
            else
                return mess;
        }

        public async Task DeleteNotSQLByModelAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity null");
            }
            entity.IsDelete = true;
            entity.DateDeleted = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<string> DeleteNotSQLByIDAsync(Guid id)
        {
            var mess = "Fail";
            var result = await entities.FirstOrDefaultAsync(x => x.ID == id && x.IsDelete == null);
            if (result != null)
            {
                result.IsDelete = true;
                result.DateDeleted = DateTime.Now;
                await _context.SaveChangesAsync();
                mess = "Success";
                return mess;
            }
            else
                return mess;
        }
    }
}
