using ApplicationTracker.Services.Dashboard;
using ApplicationTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationTracker.Controllers;

public class DashboardController : Controller
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
#pragma warning disable CS0618
        var dashboard = await _dashboardService.GetDashboardAsync();
#pragma warning restore CS0618

        return View(new DashboardViewModel { Data = dashboard });
    }
}
