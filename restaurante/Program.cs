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
using System.Diagnostics;

namespace restauranteCsharp.restaurante
{
    public class Program
    {
        private static void Main()
        {
            Console.WriteLine("-- EMPEZANDO SERVICIO --");
            LimpiezaTxt();

            Console.WriteLine("-- SERVICIO FINALIZADO --");
        }

        private static void LimpiezaTxt()
        {
            string d1 = AppDomain.CurrentDomain.BaseDirectory;
            string parent = Directory.GetParent(d1).Parent.Parent.Parent.FullName;
            string directory = $"{parent}{Path.DirectorySeparatorChar}data";
            string path = $"{directory}{Path.DirectorySeparatorChar}pagos.txt";

            //Console.WriteLine(path);
            if (!Directory.Exists(directory))
            {
                Console.WriteLine("Creando directorio.");
                Directory.CreateDirectory(directory);
                Console.WriteLine("Directorio creado.");
            }
            if (!File.Exists(path))
            {
                Console.WriteLine($"Creando archivo.");
                var f = File.Create(path);
                f.Close();
                Console.WriteLine("Archivo creado.");
            }
            
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("");
                writer.Close();
            }
        }
    }
}