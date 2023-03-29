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
        const int MAX = 10;

        Queue<Plato> PlatosPreparados = new();


        public Plato Get()
        {
            lock (this)
            {
                while (!PlatosPreparados.Any())
                {
                    Thread.Sleep(500);
                }

                var plato = PlatosPreparados.Dequeue();

                Console.WriteLine("El camarero: " + Environment.CurrentManagedThreadId + " ha recogido el plato: " + plato);
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
                Console.WriteLine("El cocinero: " + Environment.CurrentManagedThreadId + " ha preparado el plato: " + entity);

            }
        }
    }
}
