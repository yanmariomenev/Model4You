using Microsoft.AspNetCore.Mvc;

namespace Model4You.Web.Areas.Administration.Controllers
{
    public class BlogController : AdministrationController
    {
        // GET
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name)
        {
            return View();
        }
    }
}