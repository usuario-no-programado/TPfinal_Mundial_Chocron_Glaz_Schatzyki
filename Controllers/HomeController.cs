using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TPfinal_Mundial_Chocron_Glaz_Schatzyki.Models;
using Dapper;
using Microsoft.Data.SqlClient; 

namespace TPfinal_Mundial_Chocron_Glaz_Schatzyki.Controllers;

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

    public IActionResult Album()
    {
        return View();
    }

    public IActionResult Coleccion()
    {
        BD bd = new BD();
        List<Coleccion> coleccion = bd.GetColeccion();
        ViewBag.Coleccion = coleccion;
        return View();
    }

    [HttpPost]
    public IActionResult PegarFigurita(int figuritaID)
    {
        BD bd = new BD();
        bd.pegarFigurita(figuritaID);
        return RedirectToAction("Coleccion");
    }

    public IActionResult Sobres()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
