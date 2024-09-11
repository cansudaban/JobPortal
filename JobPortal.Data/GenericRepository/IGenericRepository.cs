using System.Linq.Expressions;

namespace JobPortal.Data.GenericRepository
{
    public interface IGenericRepository<T> where T : BaseEntities
    {
        /// <summary>
        /// Retrieves an entity from the database that matches the given predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The entity that matches the predicate or null if no match is found.</returns>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retrieves all entities from the database that match the given predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A collection of entities that match the predicate.</returns>
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <returns>A list of entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets an entity by ID asynchronously.
        /// </summary>
        /// <param name="id">The entity ID.</param>
        /// <returns>The entity or null if not found.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(T entity);

        /// <summary>
        /// Marks an entity as deleted.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);
    }
}
