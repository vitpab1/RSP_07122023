using Entidades.Enumerados;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.MetodosDeExtension;
using System.Text;
using Entidades.DataBase;

namespace Entidades.Modelos
{
    public class Hamburguesa : IComestible
    {
        private double costo;
        private static int costoBase;
        private bool esDoble;
        private bool estado;
        private string imagen;
        List<EIngrediente> ingredientes;
        Random random;

        public bool Estado { get => this.estado; }
        public string Imagen { get { return this.imagen; } }
        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";

        static Hamburguesa() => Hamburguesa.costoBase = 1500;
        public Hamburguesa() : this(false) { }
        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
        }


        private void AgregarIngredientes()
        {
            ingredientes = new List<EIngrediente>(random.IngredientesAleatorios());
        }



        public void FinalizarPreparacion(string cocinero)
        {
            this.costo = this.ingredientes.CalcularCostoIngredientes(costoBase);
            this.estado = true;
        }

        public void IniciarPreparacion()
        {
            if (!this.estado)
            {
                int numeroRandom = random.Next(1, 9);
                string seleccionComida = $"Hamburguesa_{numeroRandom}";
                this.imagen = DataBaseManager.GetImagenComida(seleccionComida);
                AgregarIngredientes();
            }
        }
        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));
            return stringBuilder.ToString();
        }

        public override string ToString() => this.MostrarDatos();

        
    }
}