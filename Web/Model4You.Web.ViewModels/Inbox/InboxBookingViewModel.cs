﻿namespace Model4You.Web.ViewModels.Inbox
{
    using System.Collections.Generic;

    public class InboxBookingViewModel
    {
        public IEnumerable<InboxViewModel> InboxViewModels { get; set; }

        // Pagination =>
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