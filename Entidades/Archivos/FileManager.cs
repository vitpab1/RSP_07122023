using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace Entidades.Files
{

    public static class FileManager
    {
        private static string path;

        static FileManager()
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "VITTAR.PABLO");
            ValidaExistenciaDeDirectorio();
        }

        /// <summary>
        /// Verifica si existe el directorio. Caso contrario, creara uno nuevo. 
        /// </summary>
        /// <exception cref="FileManagerException"></exception>
        private static void ValidaExistenciaDeDirectorio()
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                throw new FileManagerException("Error al crear el directorio", ex);
            }
        }

        /// <summary>
        /// Guarda los datos pasados por parametros en un archivo de texto
        /// </summary>
        /// <param name="data">Contenido a guardar</param>
        /// <param name="append">True agrega al final, false borra lo anterior</param>
        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            try
            {       
                string rutaCompleta = Path.Combine(path, nombreArchivo);
                using (StreamWriter escritor = new StreamWriter(rutaCompleta, append))
                {
                    escritor.WriteLine(data);
                }
            }
            catch (Exception ex)
            {
                throw new FileManagerException("ERROR al guardar el archivo", ex);
            }
        }

        /// <summary>
        /// Serializa a JSON el elemento pasado por parametro
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <returns>True si la serializacion fue exitosa </returns>
        /// <exception cref="FileManagerException"></exception>
        public static bool Serializar<T>(T elemento, string nombreArchivo) where T : class
        {
            try
            {
                JsonSerializerOptions opciones = new JsonSerializerOptions();
                opciones.WriteIndented = true;
                string json = JsonSerializer.Serialize(elemento, opciones);
                FileManager.Guardar(json, nombreArchivo + ".json", false);
                return true;
            }
            catch (Exception ex)
            {
                throw new FileManagerException("ERROR al serializar el archivo" , ex);
            }
        }

    }

}
