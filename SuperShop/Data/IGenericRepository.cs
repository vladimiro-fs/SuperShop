using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface IGenericRepository<T> where T : class                              // T represents a generic class in this case
    {
        /// <summary>
        /// Method that returns all entities that T is using 
        /// </summary>
        /// <returns>All entities that T is using</returns>
        IQueryable<T> GetAll();
        
        Task<T> GetByIdAsync(int id);                                                 // Get by id, receiving an id

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id); 
    }
}
