using App.Metrics.Concurrency;

namespace restauranteCsharp.restaurante.producers
{
    internal class Cocinero
    {
        private static AtomicInteger NumeroCocineros = new(0);
        public int Id;
        public string Name;

    }
}
