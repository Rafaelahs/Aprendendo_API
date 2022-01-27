using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data.Context;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly MyContext _context;
        private DbSet<T> _dataset;



        public BaseRepository(MyContext context)
        {
            _context = context;
            _dataset = _context.Set<T>();
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            try
            {
                var result = await _dataset.SingleOrDefaultAsync(p => p.id.Equals(Id));
                if (result == null)
                    return false;

                _dataset.Remove(result);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<T> InsertAsync(T item)
        {
            try
            {
                if (item.id == Guid.Empty)
                {
                    item.id = Guid.NewGuid();
                }
                item.CreatAt = DateTime.UtcNow;
                _dataset.Add(item);

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return item;
        }

        public async Task<bool> ExistAsync(Guid Id)
        {
            return await _dataset.AnyAsync(p => p.id.Equals(Id));
        }

        public async Task<T> SelectAsync(Guid Id)
        {
            try
            {
                return await _dataset.SingleOrDefaultAsync(p => p.id.Equals(Id));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
            try
            {
                return await _dataset.ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<T> UpdateAsync(T item)
        {
            try
            {
                var result = await _dataset.SingleOrDefaultAsync(p => p.id.Equals(item.id));
                if (result == null)
                    return null;

                item.UpdateAt = DateTime.UtcNow;
                item.CreatAt = result.CreatAt;

                _context.Entry(result).CurrentValues.SetValues(item);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return item;
        }
    }
}
