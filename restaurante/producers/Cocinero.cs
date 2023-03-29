using App.Metrics.Concurrency;
using restauranteCsharp.restaurante.model;
using restauranteCsharp.restaurante.monitor;

namespace restauranteCsharp.restaurante.producers
{
    internal class Cocinero : ICocinero<Plato>
    {
        private static AtomicInteger NumeroCocineros = new(0);
        public int Id;
        public string Name;
        private readonly Barra Barra;
        private readonly int Delay = new Random().Next(1000, 1500);

        public Cocinero(string name)
        {
            Id = NumeroCocineros.GetAndIncrement();
            Name = name;
            Barra = Barra.GetInstance();
        }

        public void Cocinar()
        {
            while (true)
            {
                Thread.Sleep(Delay);
                Plato Plato = new();
                Barra.Put(Plato);

                Console.WriteLine("\t-> Cocinero: " + Name + " ha preparado el plato: " + Plato);
            }
        }
    }
}
