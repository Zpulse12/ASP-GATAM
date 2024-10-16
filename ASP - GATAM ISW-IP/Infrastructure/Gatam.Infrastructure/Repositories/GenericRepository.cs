﻿using Gatam.Application.Interfaces;
using Gatam.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> Create(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task Delete(T entity)
        {
             _dbSet.Remove(entity);
             await _context.SaveChangesAsync();

        }

        public async Task<T?> FindById(string id)
        {
            var response = await _dbSet.FindAsync(id);
            return response;
        }

        public async Task<T> FindByName(string name)
        {
            return (await _dbSet.FindAsync(name))!;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return  await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        { 
            IQueryable<T> query = _dbSet; 
            foreach (var include in includes)
            {
                query = query.Include(include);
            } 
            return await query.ToListAsync();
        }

        public Task<T> Update(T entity)
        {
            EntityEntry<T> response = _dbSet.Update(entity);
            return Task.FromResult(response.Entity);
        }
    }
}
