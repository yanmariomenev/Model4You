using Model4You.Web.ViewModels.Blog;
using Model4You.Web.ViewModels.Search;

namespace Model4You.Web.ViewModels.ModelViews
{
    using System.Collections.Generic;

    public class IndexProfileViewModel
    {
        public IEnumerable<ModelProfileView> ModelProfile { get; set; }

        public IEnumerable<BlogViewModel> BlogViewModels { get; set; }

        public int Count { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public SearchInputModel SearchInputModel { get; set; }

        public int NextPage
        {
            get
            {
                if (this.CurrentPage >= this.PagesCount)
                {
                    return 1;
                }

                return this.CurrentPage + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (this.CurrentPage <= 1)
                {
                    return this.PagesCount;
                }

                return this.CurrentPage - 1;
            }
        }

    }
}