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
        BD bd = new BD();
        List<Seleccion> selecciones = bd.obtenerSelecciones();
        ViewBag.Selecciones = selecciones;

        int totalFiguritas = 864;
        int owned = 0;

        foreach (Seleccion sele in selecciones)
        {
            if (sele.Jugadores != null)
            {
                foreach (Figurita figu in sele.Jugadores)
                {
                    if (figu.Pegada)
                    {
                        owned++;
                    }
                }
            }
        }

        int porcentaje = totalFiguritas == 0 ? 0 : (int)Math.Round((owned * 100.0) / totalFiguritas);

        ViewBag.TotalFiguritas = totalFiguritas;
        ViewBag.Owned = owned;
        ViewBag.Porcentaje = porcentaje;

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
        BD bd = new BD();
        List<Figurita> recibidas = bd.abrirSobre();
        ViewBag.recibidas = recibidas;
        return View();
    }

    public IActionResult PegarTodo()
    {
        BD bd = new BD();
        bd.pegarTodo();
        return RedirectToAction("Coleccion");
    }

    public IActionResult TirarRepes()
    {
        BD bd = new BD();
        bd.tirarRepes();
        return RedirectToAction("Coleccion");
    }

    [HttpPost]
    public IActionResult Guardar(List<Figurita> recibidas)
    {
        if (recibidas != null && recibidas.Count > 0)
        {
            BD bd = new BD();
            bd.ingresarPaquete(recibidas);
        }

        return RedirectToAction("Sobres");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
