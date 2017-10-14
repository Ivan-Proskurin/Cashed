using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Cashed.Contract
{
    public interface ICommonModelQueries<T> where T : class
    {
        Task<List<T>> GetAll(bool includeDeleted = false);
        Task<T> GetById(int id);
        Task<T> GetByName(string name, bool includeDeleted = false);
    }
}
