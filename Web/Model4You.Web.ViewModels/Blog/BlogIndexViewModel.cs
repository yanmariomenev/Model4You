﻿namespace Model4You.Web.ViewModels.Blog
{
    using System.Collections.Generic;

    public class BlogIndexViewModel
    {
        public IEnumerable<BlogViewModel> BlogViewModels { get; set; }

        public IEnumerable<BlogViewModel> SideBlogViewModels { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

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