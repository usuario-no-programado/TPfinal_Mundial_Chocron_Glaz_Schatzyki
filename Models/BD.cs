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
        List<Figurita> todasFigus = new List<Figurita>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM figurita";
            todasFigus = connection.Query<Figurita>(query).ToList();
        }
        return todasFigus;
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
            int idRandom = random.Next(0, 865); // Genera un número aleatorio entre 0 y 864, cambiar en caso de agregar jugadores
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
            foreach (Seleccion sele in selecciones)
            {
                query = "SELECT * FROM figurita WHERE seleccionID = @ID";
                sele.Jugadores = connection.Query<Figurita>(query, new { ID = sele.ID }).ToList();

                foreach (Figurita figu in sele.Jugadores)
                {
                query = "SELECT pegado FROM [usuario-figurita] WHERE figuritaID = @ID AND usuarioID = 1";
                int pegado = connection.QueryFirstOrDefault<int>(query, new { ID = figu.ID });
                figu.Pegada = (pegado == 1);
                }
            }
        }
        return selecciones;
    }

    public void tirarRepes()
    {
        List<Coleccion> repes = GetColeccion();
        foreach (Coleccion rep in repes)
            {
                if (rep.Pegado && rep.CantidadFiguritasSueltas > 0)
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {  
                        query = "UPDATE [usuario-figurita] SET cantidadFiguritasSueltas = 0 WHERE figuritaID = @ID AND usuarioID = 1";
                        connection.Execute(query, new { ID = rep.FiguritaID });   
                    }
                }
            }
    }

    public void pegarTodo()
    {
        List<Coleccion> coleccion = GetColeccion();
        foreach (Coleccion item in coleccion)
        {
            pegarFigurita(item.FiguritaID);
        }
    }
}