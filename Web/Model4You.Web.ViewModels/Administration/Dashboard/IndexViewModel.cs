namespace Model4You.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;
    using Model4You.Web.ViewModels.Home.ContactView;

    public class IndexViewModel
    {
        public IEnumerable<ContactFormDataView> AnsweredQuestions { get; set; }

        public IEnumerable<ContactFormDataView> UnAnsweredQuestions { get; set; }
    }
}
