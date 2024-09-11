using JobPortal.Data.GenericRepository;

namespace JobPortal.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JobPortalDbContext _context;

        public UnitOfWork(JobPortalDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a generic repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <returns>The generic repository for the entity type.</returns>
        public IGenericRepository<T> GenericRepository<T>() where T : BaseEntities
        {
            return new GenericRepository<T>(_context);
        }

        /// <summary>
        /// Saves changes asynchronously to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
