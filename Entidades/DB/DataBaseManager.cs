using System.Data.SqlClient;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {

        private static SqlConnection connection;
        private static string stringConnection;
        static DataBaseManager()
        {
            stringConnection = "Server=.;Database=20230622SP;Trusted_Connection=True;";
        }

        /// <summary>
        /// Guarda el nombre del empleado junto con un ticket con la comida preparada 
        /// </summary>

        /// <returns> True en caso de que el ticket se haya guardado, caso contrario arroja una excepcion </returns>
        /// <exception cref="DataBaseManagerException"></exception>
        public static bool GuardarTicket<T>(string nombreEmpleado, T comida) where T : IComestible, new()
        {
            try
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string query = "INSERT INTO tickets (empleado,ticket)" +
                            "values (@empleado,@ticket)";

                    SqlCommand command = new SqlCommand(query, DataBaseManager.connection);
                    command.Parameters.AddWithValue("empleado", nombreEmpleado);
                    command.Parameters.AddWithValue("ticket", comida.Ticket);
                    DataBaseManager.connection.Open();
                    command.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new DataBaseManagerException("ERROR al intentar ESCRIBIR en la base de datos", ex );
            }
        }


        /// <summary>
        /// Devuelve la imagen de la comida seleccionada 
        /// </summary>
        /// <param name="tipo">Tipo de comida a buscar</param>
        /// <returns>Devuelve la url asociada a la comida buscada</returns>
        /// <exception cref="ComidaInvalidaExeption"></exception>
        /// <exception cref="DataBaseManagerException"></exception>
        public static string GetImagenComida(string tipo)
        {
            try
            {
                using (connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string query = "SELECT imagen FROM comidas WHERE tipo_comida=@tipo_comida";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("tipo_comida", tipo);
                    DataBaseManager.connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return reader.GetString(0);
                    }
                    else
                    {
                        throw new ComidaInvalidaExeption("ERROR: ingreso de comida invalido");
                    }
                }
            }

            catch (Exception ex)
            {
                throw new DataBaseManagerException("ERROR: Al intentar LEER de la base de datos", ex);
            }
        }


    }
}
