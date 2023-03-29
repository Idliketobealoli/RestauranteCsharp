namespace restauranteCsharp.restaurante.utils
{
    internal class DirectoryManager
    {
        public void LimpiezaTxt()
        {
            var path = GetFile();
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.WriteLine("");
                writer.Close();
            }
        }

        public void AppendText(string text)
        {
            var path = GetFile();
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(text);
                writer.Close();
            }
        }

        public List<double> FilterLines()
        {
            var path = GetFile();
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

        public string GetFile()
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
    }
}
