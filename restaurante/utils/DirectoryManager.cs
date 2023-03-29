using restauranteCsharp.restaurante.model;
using System.Text;

namespace restauranteCsharp.restaurante.utils
{
    internal class DirectoryManager
    {
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
            using (StreamWriter writer = new(path, true))
            {
                writer.WriteLine(text);
                writer.Close();
            }
        }

        public void AppendInCSV(string[] info)
        {
            String separator = ";";
            var path = GetFileCsv();
            using (StreamWriter writer = new(path, true))
            {
                writer.WriteLine(string.Join(separator, info));
                writer.Close();
            }
        }

        public string PrepareCsv()
        {
            String separator = ";";
            String[] headings = { "Camarero", "Plato", "Mesa", "Precio" };
            StringBuilder output = new();
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
                    if (value != null && Double.TryParse(value.Trim(), out num))
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
            return path;
        }
        public string GetFileCsv()
        {
            string d1 = AppDomain.CurrentDomain.BaseDirectory;
            string parent = Directory.GetParent(d1).Parent.Parent.Parent.FullName;
            string directory = $"{parent}{Path.DirectorySeparatorChar}data";
            string path = $"{directory}{Path.DirectorySeparatorChar}pagos.csv";

            //Console.WriteLine(path);
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
