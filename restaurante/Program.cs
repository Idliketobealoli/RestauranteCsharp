/*
 * Restaurante: Tenemos unos cocineros que crean platos durante un tiempo indefinido. 
 * Tenemos unos clientes que piden platos (tres por cliente, primero, segundo y postre).
 * Los camareros sirven los platos a los clientes, siempre y cuando estos estén hechos, 
 * y pueden servir hasta 3 platos a la vez.
 * Cuando los clientes terminan, pagan la cuenta.
 * Calcular el dinero total generado de 10 clientes cuando terminen. Hay 2 camareros y 3 cocineros.
 * Lo que pagan los clientes se guarda en un fichero "pagos.txt" y al finalizar los clientes, 
 * se calcula el total de dinero generado.
 */
using restauranteCsharp.restaurante.consumers;
using restauranteCsharp.restaurante.producers;
using restauranteCsharp.restaurante.utils;
using System.Diagnostics;

namespace restauranteCsharp.restaurante
{
    public class Program
    {
        private static void Main()
        {
            const int NUM_COCINEROS = 3;
            const int NUM_CAMAREROS = 2;

            Stopwatch stopwatch = new Stopwatch();
            CancellationTokenSource CancellationToken = new();

            stopwatch.Start();

            Console.WriteLine("-- EMPEZANDO SERVICIO --");
            DirectoryManager dm = new();
            dm.LimpiezaDataAsync();

            List<Cocinero> Cocineros = new();
            List<Camarero> Camareros = new();
            List<Task> TasksCocineros = new();
            List<Task> TasksCamareros = new();

            var task1 = Task.Run(() =>
            {
                while (Cocineros.Count < NUM_COCINEROS)
                {
                    Cocineros.Add(new($"Cocinero {Cocineros.Count}"));
                }
            });
            var task2 = Task.Run(() =>
            {
                while (Camareros.Count < NUM_CAMAREROS)
                {
                    Camareros.Add(new($"Camarero {Camareros.Count}"));
                }
            });

            Task finished = Task.WhenAll(task1, task2);
            finished.Wait();
            Console.WriteLine("-- CAMAREROS Y COCINEROS INICIADOS --");

            Cocineros.ForEach(cocinero =>
            {
                var t = Task.Run(() => { cocinero.Cocinar(); }, CancellationToken.Token);
                TasksCocineros.Add(t);
            });

            Camareros.ForEach(camarero =>
            {
                var t = Task.Run(() => { camarero.TakePlatos(); });
                TasksCamareros.Add(t);
            });

            Task.WaitAll(TasksCamareros.ToArray());
            CancellationToken.Cancel();

            var precios = dm.FilterLinesAsync();
            var total = 0.0;
            precios.ForEach(prec => total += prec);

            stopwatch.Stop();

            Console.WriteLine($"-- Total de dinero generado: {Math.Round(total, 2)} --");
            Console.WriteLine($"-- Tiempo de ejecución: {stopwatch.ElapsedMilliseconds} ms --");

            Console.WriteLine("-- SERVICIO FINALIZADO --");
        }
    }
}