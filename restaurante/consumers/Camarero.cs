using App.Metrics.Concurrency;
using restauranteCsharp.restaurante.model;
using restauranteCsharp.restaurante.monitor;
using restauranteCsharp.restaurante.utils;

namespace restauranteCsharp.restaurante.consumers
{
    internal class Camarero : ICamarero<Plato>
    {

        private const int MAX_PEDIDOS = 100;

        private static AtomicInteger NumeroCamareros = new(0);
        public int Id;
        public string Name;
        private readonly Barra Barra;

        private readonly Queue<Plato> PlatosEnManos = new();
        private static int pedidoId = 0;

        private readonly int Delay = new Random().Next(100, 300);
        private readonly DirectoryManager Dm = new();

        public Camarero(string name)
        {
            Id = NumeroCamareros.GetAndIncrement();
            Name = name;
            Barra = Barra.GetInstance();
    }


        public void SendPlatos(int pedidoId)
        {
            Console.WriteLine("\t -> " + Name + " prepara el pedido: " + pedidoId);
            while (PlatosEnManos.Any())
            {
                var plato = PlatosEnManos.Dequeue();
                var line = $"{Name} sirve plato {plato.Id} a la mesa {plato.Mesa} con precio: {plato.Precio}";
                Dm.AppendText(line);
                Dm.AppendInCSV(new string[] {$"{Name}", $"{plato.Id}", $"{plato.Mesa}", $"{plato.Precio}"});
            }
            Console.WriteLine("\t -> Camarero: " + Name + " entregó el pedido: " + pedidoId);
        }

        public void TakePlatos()
        {

            while (pedidoId < MAX_PEDIDOS) {
                Thread.Sleep(Delay);

                var plato = Barra.Get();
                if (plato != null)
                {
                    Console.WriteLine("\t -> Camarero: " + Name + " obtiene el plato: " + plato.ToString());

                    PlatosEnManos.Enqueue(plato);

                    if (PlatosEnManos.Count == 3)
                    {
                        pedidoId++;
                        SendPlatos(pedidoId);
                    }
                }
            }

            Console.WriteLine("\t -> Camarero: " + Name + " termina su turno.");
        }
    }
}
