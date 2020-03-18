using System.Collections.Generic;
using System.Threading.Tasks;

namespace Model4You.Services.Data.AdminServices
{
    public interface IContactDataService
    {
        Task<IEnumerable<T>> TakeAllUnAnswered<T>();

        Task<IEnumerable<T>> TakeAllAnswered<T>();
    }
}