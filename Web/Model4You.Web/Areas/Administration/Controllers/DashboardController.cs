namespace Model4You.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Model4You.Services.Data;
    using Model4You.Services.Data.AdminServices;
    using Model4You.Web.ViewModels.Administration.Dashboard;

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
                AnsweredQuestions = await this.contactServices.TakeAllAnswered<ContactFormDataView>(),
                UnAnsweredQuestions = await this.contactServices.TakeAllUnAnswered<ContactFormDataView>(),
            };
            return this.View(viewModel);
        }

        public async Task<IActionResult> Answered(string id)
        {
            await this.contactServices.MoveToAnswered(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
