using Microsoft.AspNetCore.Mvc;

namespace Model4You.Web.Controllers
{
    public class BlogController : Controller
    {
        // GET
        public IActionResult Blog()
        {
            return View();
        }
    }
}