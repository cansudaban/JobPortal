using JobPortal.Data.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobPortal.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntities
    {
        private readonly JobPortalDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(JobPortalDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate) 
{
    return await _dbSet.Where(predicate).ToListAsync();
}

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }
    }
}
