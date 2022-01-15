using Microsoft.AspNetCore.Mvc;

namespace My_Fuel.Controllers
{
    public class VisualizarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
