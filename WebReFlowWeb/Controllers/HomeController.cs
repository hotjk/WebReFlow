using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using System.Diagnostics;
using WebReFlowWeb.Models;
using WebReFlow;

namespace WebReFlowWeb.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Route("reflow")]
    public async Task<IActionResult> Reflow()
    {
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        await using var page = await browser.NewPageAsync();

        var path = "file:///" + Path.Combine(Directory.GetCurrentDirectory(), "test.html");
        await page.GoToAsync(path);

        var tables = await page.ExtractTable();
        var characters = await page.ExtractCharacter();
        characters.Shuffle();

        return View((characters, tables));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
