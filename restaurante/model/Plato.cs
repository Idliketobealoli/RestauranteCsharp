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
        public Plato()
        {
            Id = NumeroPlatos.GetAndAdd(1);
            Mesa = new Random().Next(1, 11);
            Precio = Math.Round(new Random().NextDouble()*(25.5-10.0)+10.0, 2);
            TipoPlato = GenerateTipo();
        }

        private static Tipo GenerateTipo()
        {
            var tipoNum = new Random().Next(1, 4);
            if (tipoNum == 1) return Tipo.PRIMERO;
            else if (tipoNum == 2) return Tipo.SEGUNDO;
            else return Tipo.POSTRE;
        }

        public override string ToString()
        {
            return $"Plato[Id={Id},Mesa={Mesa},Precio={Precio},TipoPlato={TipoPlato}]";
        }
    }

    enum Tipo
    {
        PRIMERO,
        SEGUNDO,
        POSTRE
    }
}
