using App.Metrics.Concurrency;

namespace restauranteCsharp.restaurante.model
{
    internal class Plato
    {
        private AtomicInteger NumeroPlatos = 0;
        public int Id { get; set; }
        public int Mesa { get; set; }
        public double Precio { get; set; }
        public Tipo TipoPlato { get; set; }
        public Plato(int mesa, double precio, Tipo tipo) 
        {
            Id = NumeroPlatos;
            NumeroPlatos++;

        }
    }

    enum Tipo
    {
        PRIMERO,
        SEGUNDO,
        POSTRE
    }
}
