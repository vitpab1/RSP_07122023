using Entidades.DataBase;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;

namespace Entidades.Modelos
{
    public delegate void DelegadoDemoraAtencion(double demora);
    public delegate void DelegadoPedidoEnCurso(IComestible menu);

    public class Cocinero<T> where T : IComestible, new()
    {
        private CancellationTokenSource cancellation;

        private int cantPedidosFinalizados; 
        private double demoraPreparacionTotal;
        private T pedidoEnPreparacion;
        private string nombre;
        private Task tarea;

        private Mozo<T> mozo;
        private Queue<T> pedidos;

        public event DelegadoPedidoEnCurso OnPedido;
        public event DelegadoDemoraAtencion OnDemora;



        public Cocinero(string nombre)
        {
            this.nombre = nombre;
            mozo = new Mozo<T>();
            pedidos = new Queue<T>();
            this.mozo.OnPedido += this.TomarNuevoPedido;

        }

        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                    this.tarea.Status == TaskStatus.WaitingToRun ||
                    this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.mozo.EmpezarATrabajar = true; 
                    this.EmpezarACocinar();
                }
                else
                {
                    this.mozo.EmpezarATrabajar = !this.mozo.EmpezarATrabajar;

                    this.cancellation.Cancel();
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }

        public Queue<T> Pedidos { get => this.pedidos; }

        /// <summary>
        /// En un hilo secundario, ejecuta NotificarNuevoIngreso, EsperarProximoIngreso, GuardarTicket e incrementar en 1 la cantidad de pedidos finalizados
        /// </summary>
        private void EmpezarACocinar()
        {
            this.tarea = Task.Run(() =>
            {
                while (!this.cancellation.IsCancellationRequested)
                {
                    if(pedidos.Count != 0)
                    {
                        this.pedidoEnPreparacion = pedidos.Dequeue();
                        this.EsperarProximoIngreso();
                        this.cantPedidosFinalizados++;
                        DataBaseManager.GuardarTicket(this.nombre, this.pedidoEnPreparacion);

                    }
                   
                }
            }, this.cancellation.Token);
        }

        /// <summary>
        /// Incrementa de a 1 seg el tiempo de demora
        /// </summary>
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;

            if (this.OnDemora != null)
            {
                while (!this.pedidoEnPreparacion.Estado  && !this.cancellation.IsCancellationRequested)
                {
                    Thread.Sleep(1000);
                    tiempoEspera++;
                    this.OnDemora.Invoke(tiempoEspera);
                }
            }

            this.demoraPreparacionTotal += tiempoEspera;
        }

        private void TomarNuevoPedido(T menu)
        {
            if (this.OnPedido is not null)
            {
                this.pedidos.Enqueue(menu);
            }
        }
    }
}