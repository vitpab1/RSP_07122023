using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Modelos;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {
            //arrange
            string testTexto = "Test verificacion nombre de archivo invalido en metodo Guardar()";
            string nombreArchivo = "invalid/path.txt"; 

            //act
            FileManager.Guardar(testTexto, nombreArchivo, true);

        }

        [TestMethod]

        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            //arrange
            string nombreCocinero = "Carlos Test";
            Cocinero<Hamburguesa> cocinero = new Cocinero<Hamburguesa>(nombreCocinero);

            //act
            int pedidosFinalizados = cocinero.CantPedidosFinalizados;

            //assert
            Assert.AreEqual(0, pedidosFinalizados);
        }
    }
}