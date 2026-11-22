using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("Error")]
    public IActionResult Error()
    {
        return View();
    }

    [Route("not-found")]
    public IActionResult NotFoundPage()
    {
        return View("NotFound");
    }
}
