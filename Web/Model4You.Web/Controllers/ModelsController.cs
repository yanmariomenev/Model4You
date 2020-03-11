using Microsoft.AspNetCore.Mvc;

namespace Model4You.Web.Controllers
{
    public class ModelsController : Controller
    {
        // GET
        public IActionResult Model()
        {
            return View();
        }
    }
}