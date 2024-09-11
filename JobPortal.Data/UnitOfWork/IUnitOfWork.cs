using JobPortal.Data;
using JobPortal.Data.GenericRepository;

namespace JobPortal.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets a generic repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <returns>The generic repository for the entity type.</returns>
        IGenericRepository<T> GenericRepository<T>() where T : BaseEntities;

        /// <summary>
        /// Saves changes asynchronously to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync();
    }

}
