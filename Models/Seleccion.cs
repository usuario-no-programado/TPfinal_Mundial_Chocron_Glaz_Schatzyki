namespace TPfinal_Mundial_Chocron_Glaz_Schatzyki.Models;

public class Seleccion
{
    public int ID { get; set; }
    public string Nacion { get; set; }
    public List<jugadores> Jugadores { get; set; }
}