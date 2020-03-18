using System.Threading.Tasks;
using Model4You.Services.Data.AdminServices;

namespace Model4You.Web.Areas.Administration.Controllers
{
    using Model4You.Services.Data;
    using Model4You.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly IContactDataService contactServices;

        public DashboardController(IContactDataService contactServices)
        {
            this.contactServices = contactServices;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel
            {
                AnsweredQuestions = await contactServices.TakeAllAnswered<ContactFormDataView>(),
                UnAnsweredQuestions = await contactServices.TakeAllUnAnswered<ContactFormDataView>(),
            };
            return this.View(viewModel);
        }
    }
}
