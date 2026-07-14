using Dapper;
using Microsoft.Data.SqlClient;

namespace TPfinal_Mundial_Chocron_Glaz_Schatzyki.Models;

public class BD {
    private string _connectionString = @"Server=localhost;DataBase=Album;Integrated Security=True;TrustServerCertificate=True;";
    string query;

    public void ingresarPaquete(List<Figurita> nuevas){

        foreach (Figurita id in nuevas){
            List<int> numeros = new List<int>();
            query = "SELECT figuritaID FROM [usuario-figurita]";
            using (SqlConnection conection = new SqlConnection(_connectionString)){
                numeros = conection.Query<int>(query).ToList();
            }
            if(numeros.Contains(id.ID)){
                using (SqlConnection connection = new SqlConnection(_connectionString)){
                    query = "UPDATE [usuario-figurita] SET cantidadFiguritasSueltas = cantidadFiguritasSueltas + 1 WHERE figuritaID = @ID";
                    connection.Execute(query, new { ID = id.ID });
                }
            } 
            else 
            {
                using (SqlConnection connection = new SqlConnection(_connectionString)){
                    query = "INSERT INTO [usuario-figurita] (usuarioID, figuritaID, cantidadFiguritasSueltas, pegado) VALUES (1, @ID, 1, 0)";
                    connection.Execute(query, new { ID = id.ID });
                }
            }
        }
    }

    public void pegarFigurita(int figuritaID)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            query = "SELECT pegado FROM [usuario-figurita] WHERE figuritaID = @ID AND usuarioID = 1";
            int pegado = connection.QueryFirstOrDefault<int>(query, new { ID = figuritaID });
            
            if (pegado == 0)
            {
                query = "UPDATE [usuario-figurita] SET pegado = 1, cantidadFiguritasSueltas = cantidadFiguritasSueltas - 1 WHERE figuritaID = @ID AND usuarioID = 1";
                connection.Execute(query, new { ID = figuritaID });
            }
        }
    }

    public List<Figurita> obtenerTodasLasFiguritas()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM figurita";
            return connection.Query<Figurita>(query).ToList();
        }
    }

    public List<Coleccion> GetColeccion(){
        List<Coleccion> coleccion = new List<Coleccion>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT [usuario-figurita].figuritaID, [usuario-figurita].cantidadFiguritasSueltas, [usuario-figurita].pegado FROM [usuario-figurita] inner join figurita on figurita.ID = [usuario-figurita].figuritaID WHERE [usuario-figurita].usuarioID = 1 AND [usuario-figurita].cantidadFiguritasSueltas > 0 order by figurita.ID";
            coleccion = connection.Query<Coleccion>(query).ToList();
        }

        return coleccion;
    }

    public List<Figurita> abrirSobre(){
        List<Figurita> figus = new List<Figurita>();
        List<Figurita> todasFigus = obtenerTodasLasFiguritas();
        Random random = new Random();
        
        for (int i = 0; i < 5; i++)
        {
            int idRandom = random.Next(0, todasFigus.Count);
            figus.Add(todasFigus[idRandom]);
        }
        
        return figus;
    }

    public List<Seleccion> obtenerSelecciones()
    {
        List<Seleccion> selecciones = new List<Seleccion>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM seleccion";
            selecciones = connection.Query<Seleccion>(query).ToList(); 
        }
        foreach (Seleccion sele in selecciones)
        {
            string query = "SELECT * FROM figurita WHERE seleccionID = @ID";
            sele.jugadores = connection.Query<Figurita>(query, new { ID = sele.ID }).ToList();
        }
        return selecciones;
    }
}