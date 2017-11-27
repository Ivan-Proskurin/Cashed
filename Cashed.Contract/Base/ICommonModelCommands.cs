using System.Threading.Tasks;

namespace Cashed.Logic.Contract.Base
{
    public interface ICommonModelCommands<T> where T : class
    {
        Task<T> Update(T model);
        Task Delete(int id, bool onlyMark = true);
    }
}
