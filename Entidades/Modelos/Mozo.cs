﻿using Entidades.DataBase;
using Entidades.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public delegate void DelegadoNuevoPedido<T>(T menu);

    public class Mozo<T> where T : IComestible, new()
    {
        private CancellationTokenSource cancellation;
        private T menu;
        private Task tarea;

        public event DelegadoNuevoPedido<T> OnPedido;


        public bool EmpezarATrabajar
        {
            get
            {
                return this.tarea != null && (this.tarea.Status == TaskStatus.Running ||
                  this.tarea.Status == TaskStatus.WaitingToRun ||
                  this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.EmpezarATrabajar)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.TomarPedidos();
                }
                else
                {
                    this.cancellation.Cancel();
                }
            }
        }
  

        private void TomarPedidos()
        {
            this.tarea = Task.Run(() =>
            {
                while (!this.cancellation.IsCancellationRequested)
                {
                    NotificarNuevoPedido();
                    Thread.Sleep(5000);
                }
            }, this.cancellation.Token);
        }

       
        private void NotificarNuevoPedido()
        {
            if (this.OnPedido != null)
            {
                this.menu = new T();
                this.menu.IniciarPreparacion();
                this.OnPedido.Invoke(this.menu);
            }
        }

    }
}
