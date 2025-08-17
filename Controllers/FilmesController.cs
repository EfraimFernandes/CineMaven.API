using Microsoft.AspNetCore.Mvc;

namespace CineMaven.API.Controllers
{
    public class FilmesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
