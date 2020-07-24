namespace Model4You.Services.Data.SearchService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISearchService
    {
        Task<ICollection<T>> SearchResult<T>(
            string country, string city, string gender, int age, int to);
    }
}
