using Microsoft.AspNetCore.Mvc;

namespace Model4You.Web.Controllers
{
    public class SearchController : Controller
    {
        // GET
        public IActionResult Search()
        {
            return this.View();
        }
    }
}