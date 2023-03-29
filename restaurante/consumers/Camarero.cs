using App.Metrics.Concurrency;
using restauranteCsharp.restaurante.model;
using restauranteCsharp.restaurante.monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restauranteCsharp.restaurante.consumers
{
    internal class Camarero : ICamarero<Plato>
    {

        private const int MAX_PEDIDOS = 10;

        private static AtomicInteger NumeroCamareros = new(0);
        public int Id;
        public string Name;
        private readonly Barra Barra;

        private readonly Queue<Plato> PlatosEnManos = new();
        private static int pedidoId = 0;

        private readonly int Delay = new Random().Next(700, 1000);

        public Camarero(string name) 
        {
            Id = NumeroCamareros.GetAndIncrement();
            Name = name;
            Barra = Barra.GetInstance();

        }


        public void SendPlatos(int pedidoId)
        {
            Console.WriteLine("\t -> Camarero: " + Name + " realiza el pedido: " + pedidoId);
            PlatosEnManos.Clear();
        }

        public void TakePlatos()
        {

            while (pedidoId < MAX_PEDIDOS) {
                Thread.Sleep(Delay);

                var plato = Barra.Get();
                Console.WriteLine("\t -> Camarero: " + Name + " obtiene el plato: " + plato);

                PlatosEnManos.Enqueue(plato);

                if (PlatosEnManos.Count == 3)
                {
                    pedidoId++;
                    SendPlatos(pedidoId);
                }
            }

            Console.WriteLine("\t -> Camarero: " + Name + " termina su turno.");
        }
    }
}
