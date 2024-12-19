using Microsoft.EntityFrameworkCore;
using BillCollector.Core.Entities;
using BillCollector.Infrastructure.DbContexts;
using BillCollector.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillCollector.Infrastructure.Repository
{
    public class GenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly BillCollectorDbContext _context;
        private DbSet<TEntity> DbSet;

        public GenericRepository(BillCollectorDbContext context)
        {
            _context = context;
            DbSet = _context.Set<TEntity>();
        }

        /// <summary>
        /// Insert single record
        /// </summary>
        /// <param name="entity">entity to process</param>
        /// <param name="commitTransaction">default is true. If set to false, unitofwork.CommitAsync() must be called manually</param>
        /// <returns></returns>
        public async Task<TEntity> InsertAsync(TEntity entity, bool commitTransaction = true)
        {
            try
            {
                entity.CreatedAt = DateTime.Now;
                entity.IsDeleted = false;
                var result = DbSet.Add(entity);
                if(commitTransaction)
                {
                    await _context.SaveChangesAsync();
                }
                return result.Entity;
            }
            catch (Exception)
            {
                _context.ChangeTracker.Clear();
                throw;
            }
        }

        /// <summary>
        /// Insert multiple record
        /// </summary>
        /// <param name="entities">entities to process</param>
        /// <param name="commitTransaction">default is true. If set to false, unitofwork.CommitAsync() must be called manually</param>
        /// <returns></returns>
        public async Task<List<TEntity>> InsertAsync(List<TEntity> entities, bool commitTransaction = true)
        {
            try
            {
                entities.ForEach(e => 
                                {
                                    e.CreatedAt = DateTime.Now;
                                    e.IsDeleted = false;
                                });
                DbSet.AddRange(entities);
                if (commitTransaction)
                {
                    await _context.SaveChangesAsync();
                }
                return entities;
            }
            catch (Exception)
            {
                _context.ChangeTracker.Clear();
                throw;
            }
        }

        /// <summary>
        /// Update single record
        /// </summary>
        /// <param name="entity">entity to process</param>
        /// <param name="commitTransaction">default is true. If set to false, unitofwork.CommitAsync() must be called manually</param>
        /// <returns></returns>
        public async Task<TEntity> UpdateAsync(TEntity entity, bool commitTransaction = true)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                entity.LastModifiedAt = DateTime.Now;
                DbSet.Update(entity);

                if (commitTransaction)
                {
                    int rowsAffected = await _context.SaveChangesAsync();
                    if (rowsAffected > 0)
                    {
                        return entity;
                    }
                    return null;
                }

                return entity;
            }
            catch (Exception ex)
            {
                _context.ChangeTracker.Clear();
                throw;
            }
        }

        /// <summary>
        /// Update multiple record
        /// </summary>
        /// <param name="entities">entities to process</param>
        /// <param name="commitTransaction">default is true. If set to false, unitofwork.CommitAsync() must be called manually</param>
        /// <returns></returns>
        public async Task<List<TEntity>> UpdateAsync(List<TEntity> entities, bool commitTransaction = true)
        {
            if (entities == null || !entities.Any())
            {
                return new List<TEntity>();
            }

            try
            {
                foreach (var entity in entities)
                {
                    entity.LastModifiedAt = DateTime.Now;
                    DbSet.Update(entity);
                }

                if (commitTransaction)
                {
                    int rowsAffected = await _context.SaveChangesAsync();
                    if (rowsAffected > 0)
                    {
                        return entities;
                    }
                    return new List<TEntity>();
                }

                return entities;
            }
            catch (Exception)
            {
                _context.ChangeTracker.Clear();
                throw;
            }
        }

        public async Task<TEntity> GetByIdAsync(long Id)
        {
            try
            {
                var result = await DbSet.FirstOrDefaultAsync(p => p.Id == Id && !p.IsDeleted);
                
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public TEntity GetOne(Func<TEntity, bool> predicate)
        {
            try
            {
                return DbSet.Where(p => !p.IsDeleted)?.Where(predicate)?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            var result = await DbSet.ToListAsync();
            
            result = result.Where(p => !p.IsDeleted).ToList();

            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<List<TEntity>> GetMany(Func<TEntity, Boolean> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            var result = DbSet.Where(p=>!p.IsDeleted).Where(predicate).ToList();
            
            return result;
        }

        public (List<TEntity> records, int totalRecords) Paginate(int page, int pageSize, Func<TEntity, Boolean> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            var result = DbSet
                        .Where(p => !p.IsDeleted)
                        .Where<TEntity>(predicate)
                        .Skip((page) * pageSize)
                        .Take(pageSize)
                        .ToList();

            int count = DbSet.Where(p=>!p.IsDeleted).Count();

            return (result, count);
        }


    }
}
