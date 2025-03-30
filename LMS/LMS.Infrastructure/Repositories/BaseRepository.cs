﻿using LMS.Domain.Interfaces;
using LMS.Infrastructure.DbContexts;
using LMS.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        
        public async Task<ICollection<TEntity>> GettAllAsync(ISpecification<TEntity> specification)
        {
            var query = SpecificationQueryBuilder.GetQuery(_dbSet, specification);
            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByCriteriaAsync(ISpecification<TEntity> specification)
        {
            var query = SpecificationQueryBuilder.GetQuery(_dbSet, specification);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
                _dbSet.Update(entity);
                await SaveChangesAsync();
        }
       
        public abstract Task DeleteAsync(Guid id);

        public async Task DeleteHardlyAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity != null)
            {
                _dbSet.Remove(entity);
                await SaveChangesAsync();
            }

            else
            {
                throw new KeyNotFoundException("id is uncorrect");
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
