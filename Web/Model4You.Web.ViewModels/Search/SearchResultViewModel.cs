namespace Model4You.Web.ViewModels.Search
{
    using System.Collections.Generic;

    public class SearchResultViewModel
    {
        public ICollection<SearchViewModel> SearchViewModels { get; set; }

        public SearchInputModel SearchInputModel { get; set; }

        public string EmptyResult { get; set; }
    }
}
