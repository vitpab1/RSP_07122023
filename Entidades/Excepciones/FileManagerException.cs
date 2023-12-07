using Entidades.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Exceptions
{
    public class FileManagerException : Exception
    {
        public FileManagerException(string message) : base(message)
        {
            FileManager.Guardar(message, "logs.txt", true);

        }

        public FileManagerException(string message, Exception innerException) : base(message, innerException)
        {
            string fullMessage = $"{message}\nInner Exception: {innerException?.Message}";

            FileManager.Guardar(fullMessage, "logs.txt", true);

        }
    }
}
