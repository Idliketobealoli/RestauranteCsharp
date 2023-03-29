using App.Metrics.Concurrency;

namespace restauranteCsharp.restaurante.model
{
    internal class Plato
    {
        private static AtomicInteger NumeroPlatos = new(0);
        public int Id { get; set; }
        public int Mesa { get; set; }
        public double Precio { get; set; }
        public Tipo TipoPlato { get; set; }
        public Plato(int mesa, double precio, Tipo tipo)
        {
            Id = NumeroPlatos.GetAndAdd(1);
            Mesa = mesa;
            Precio = precio;
            TipoPlato = tipo;
        }
    }

    enum Tipo
    {
        PRIMERO,
        SEGUNDO,
        POSTRE
    }
}
