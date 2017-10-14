using System.Threading.Tasks;

namespace Logic.Cashed.Contract
{
    public interface IGenericModelCommands<T> where T : class
    {
        Task<T> Update(T model);
        Task Delete(int id, bool onlyMark = true);
    }
}
