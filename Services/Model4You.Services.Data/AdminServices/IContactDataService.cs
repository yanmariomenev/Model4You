namespace Model4You.Services.Data.AdminServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IContactDataService
    {
        Task<IEnumerable<T>> TakeAllUnAnswered<T>();

        Task<IEnumerable<T>> TakeAllAnswered<T>();
    }
}