using restauranteCsharp.restaurante.model;
using System.Collections.Concurrent;

namespace restauranteCsharp.restaurante.monitor
{
    internal class Barra : IMonitor<Plato>
    {
        private Barra() { }
        private static Barra Instance;
        private static readonly object Lock = new();

        private const int MAX = 10;
        private ConcurrentQueue<Plato> PlatosPreparados { get; set; }

        public static Barra GetInstance()
        {
            if (Instance == null)
            {
                lock (Lock)
                {
                    if (Instance == null)
                    {
                        Instance = new()
                        {
                            PlatosPreparados = new ConcurrentQueue<Plato>()
                        };
                    }
                }
            }
            return Instance;
        }

        public Plato? Get()
        {
            while (!PlatosPreparados.Any())
            {
                Thread.Sleep(750);
            }
            lock (this)
            {
                PlatosPreparados.TryDequeue(out Plato? plato);

                if (plato != null) { Console.WriteLine("Se ha recogido el plato: " + plato); }
                return plato;
            }
        }

        public void Put(Plato entity)
        {
            while (PlatosPreparados.Count == MAX)
            {
                Thread.Sleep(750);
            }
            lock (this)
            {
                PlatosPreparados.Enqueue(entity);
                Console.WriteLine("Se ha preparado el plato: " + entity);
            }
        }
    }
}
