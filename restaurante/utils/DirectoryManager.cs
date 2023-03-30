namespace restauranteCsharp.restaurante.utils
{
    internal class DirectoryManager
    {
        //private TimeSpan Timeout = TimeSpan.FromMilliseconds(500);
        private static Mutex MutexTXT = new();
        private static Mutex MutexCSV = new();
        public void LimpiezaDataAsync()
        {
            var pathTxt = GetFileTxt();

            MutexTXT.WaitOne();
            using (StreamWriter writer = new(pathTxt, false))
            {
                writer.Close();
            }
            MutexTXT.ReleaseMutex();

            var pathCsv = GetFileCsv();

            MutexCSV.WaitOne();
            using (StreamWriter writer = new(pathCsv, false))
            {
                writer.WriteLine(PrepareCsv());
                writer.Close();
            }
            MutexCSV.ReleaseMutex();
            PrepareCsv();
        }

        // el \r\n esta puesto asi para que luego el texto no de fallo de formato de salto de linea.
        public void AppendText(string text)
        {
            var path = GetFileTxt();

            MutexTXT.WaitOne();
            File.AppendAllText(path, text + "\r\n");
            MutexTXT.ReleaseMutex();
        }

        public void AppendInCSV(string[] info)
        {
            string separator = ";";
            var path = GetFileCsv();

            MutexCSV.WaitOne();
            File.AppendAllText(path, string.Join(separator, info) + "\r\n");
            MutexCSV.ReleaseMutex();
        }

        public string PrepareCsv()
        {
            string separator = ";";
            string[] headings = { "Camarero", "Plato", "Mesa", "Precio" };
            return string.Join(separator, headings);
        }

        public List<double> FilterLinesAsync()
        {
            var path = GetFileTxt();
            List<string> lines;
            List<double> res = new();
            try
            {
                MutexTXT.WaitOne();
                lines = File.ReadLines(path).ToList();
                MutexTXT.ReleaseMutex();
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

            MutexTXT.WaitOne();
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
            MutexTXT.ReleaseMutex();
            return path;
        }
        public string GetFileCsv()
        {
            string d1 = AppDomain.CurrentDomain.BaseDirectory;
            string parent = Directory.GetParent(d1).Parent.Parent.Parent.FullName;
            string directory = $"{parent}{Path.DirectorySeparatorChar}data";
            string path = $"{directory}{Path.DirectorySeparatorChar}pagos.csv";

            MutexCSV.WaitOne();
            if (!File.Exists(path))
            {
                Console.WriteLine($"Creando archivo.");
                var f = File.Create(path);
                f.Close();
                Console.WriteLine("Archivo creado.");
            }
            MutexCSV.ReleaseMutex();
            return path;
        }
    }
}
