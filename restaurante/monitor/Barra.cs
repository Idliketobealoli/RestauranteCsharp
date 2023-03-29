using restauranteCsharp.restaurante.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace restauranteCsharp.restaurante.monitor
{
    internal class Barra : IMonitor<Plato>
    {
        private Barra() { }
        private static Barra Instance;
        private static readonly object Lock = new();

        private const int MAX = 10;
        private Queue<Plato> PlatosPreparados { get; set; }

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
                            PlatosPreparados = new Queue<Plato>()
                        };
                    }
                }
            }
            return Instance;
        }

        public Plato Get()
        {
            lock (this)
            {
                while (!PlatosPreparados.Any())
                {
                    Thread.Sleep(500);
                }

                var plato = PlatosPreparados.Dequeue();

                Console.WriteLine("Se ha recogido el plato: " + plato);
                return plato;
            }
        }

        public void Put(Plato entity)
        {
            lock (this)
            {
                while (PlatosPreparados.Count == MAX)
                {
                    Thread.Sleep(750);
                }

                PlatosPreparados.Enqueue(entity);
                Console.WriteLine("Se ha preparado el plato: " + entity);

            }
        }
    }
}
