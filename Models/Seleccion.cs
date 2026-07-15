namespace TPfinal_Mundial_Chocron_Glaz_Schatzyki.Models;

public class Seleccion
{
    public int ID { get; set; }
    public string pais { get; set; }
    public string grupo { get; set; }
    public List<Figurita> Jugadores { get; set; }
}