using Dapper;
using Microsoft.Data.SqlClient;

public class BD{
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
            string query = "SELECT [usuario-figurita].figuritaID, [usuario-figurita].cantidadFiguritasSueltas, [usuario-figurita].pegado FROM [usuario-figurita] inner join figurita on figurita.ID = [usuario-figurita].figuritaID WHERE [usuario-figurita].usuarioID = 1 order by figurita.seleccionID";
            coleccion = connection.Query<Coleccion>(query).ToList();
        }

        return coleccion;
    }
}