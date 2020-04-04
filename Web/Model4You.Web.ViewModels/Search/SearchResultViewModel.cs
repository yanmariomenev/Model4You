using System.Collections.Generic;

namespace Model4You.Web.ViewModels.Search
{
    public class SearchResultViewModel
    {
        public ICollection<SearchViewModel> SearchViewModels { get; set; }

        public string EmptyResult { get; set; }
    }
}