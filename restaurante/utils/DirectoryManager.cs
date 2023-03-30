using restauranteCsharp.restaurante.model;
using System.Text;

namespace restauranteCsharp.restaurante.utils
{
    internal class DirectoryManager
    {
        private TimeSpan Timeout = TimeSpan.FromMilliseconds(500);
        public void LimpiezaData()
        {
            var pathTxt = GetFileTxt();
            using (StreamWriter writer = new(pathTxt, false))
            {
                writer.Close();
            }

            var pathCsv = GetFileCsv();
            using (StreamWriter writer = new(pathCsv, false))
            {
                writer.WriteLine(PrepareCsv());
                writer.Close();
            }

            PrepareCsv();
        }

        public void AppendText(string text)
        {
            var path = GetFileTxt();
            bool lockTXTTaken = false;
            object lockTXT = new object();
            try
            {
                Monitor.TryEnter(lockTXT, Timeout, ref lockTXTTaken);
                if (lockTXTTaken)
                {
                    using (StreamWriter writer = new(path, true))
                    {
                        writer.WriteLineAsync(text);
                        writer.Close();
                    }
                }
                else
                {
                    Console.WriteLine($"Lock not acquired by {Thread.CurrentThread.Name}.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (lockTXTTaken)
                {
                    Monitor.Exit(lockTXT);
                }
            }
        }

        public void AppendInCSV(string[] info)
        {
            string separator = ";";
            var path = GetFileCsv();
            bool lockCSVTaken = false;
            object lockCSV = new object();
            try
            {
                Monitor.TryEnter(lockCSV, Timeout, ref lockCSVTaken);
                if (lockCSVTaken)
                {
                    using (StreamWriter writer = new(path, true))
                    {
                        writer.WriteLineAsync(string.Join(separator, info));
                        writer.Close();
                    }
                }
                else
                {
                    Console.WriteLine($"Lock not acquired by {Thread.CurrentThread.Name}.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (lockCSVTaken)
                {
                    Monitor.Exit(lockCSV);
                }
            }
        }

        public string PrepareCsv()
        {
            string separator = ";";
            string[] headings = { "Camarero", "Plato", "Mesa", "Precio" };
            return string.Join(separator, headings);
        }

        public List<double> FilterLines()
        {
            var path = GetFileTxt();
            List<string> lines;
            List<double> res = new();
            try
            {
                lines = File.ReadLines(path).ToList();
                lines.ForEach(line =>
                {
                    var value = line.Split(":").LastOrDefault();
                    double num;
                    if (value != null && double.TryParse(value.Trim(), out num))
                    {
                        res.Add(num);
                    }
                });
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine($"There was a problem reading path: {path}");
                Console.WriteLine(e.ToString());
                return new();
            }
        }

        public string GetFileTxt()
        {
            string d1 = AppDomain.CurrentDomain.BaseDirectory;
            string parent = Directory.GetParent(d1).Parent.Parent.Parent.FullName;
            string directory = $"{parent}{Path.DirectorySeparatorChar}data";
            string path = $"{directory}{Path.DirectorySeparatorChar}pagos.txt";

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
            return path;
        }
        public string GetFileCsv()
        {
            string d1 = AppDomain.CurrentDomain.BaseDirectory;
            string parent = Directory.GetParent(d1).Parent.Parent.Parent.FullName;
            string directory = $"{parent}{Path.DirectorySeparatorChar}data";
            string path = $"{directory}{Path.DirectorySeparatorChar}pagos.csv";

            if (!File.Exists(path))
            {
                Console.WriteLine($"Creando archivo.");
                var f = File.Create(path);
                f.Close();
                Console.WriteLine("Archivo creado.");
            }
            return path;
        }
    }
}
