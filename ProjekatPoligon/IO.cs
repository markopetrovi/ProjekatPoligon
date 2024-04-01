using System.Xml.Serialization;

namespace Program
{
	public partial class Polygon
	{
		static private string MultilineInput()
		{
			string res = "";
			string? line;
			while ((line = Console.ReadLine()) != null && line != "\u0003")
				res = res + line + "\n";
			return res;
		}

        static private readonly char[] separator = [' ', '\t', '\n', '\r'];

        public void Print()
		{
			for (int i = 0; i < vertices.Count; i++)
				Console.WriteLine($"A{i}: ({vertices[i].x}, {vertices[i].y})");
		}
		static public List<Point>? Input()
		{
			List<Point> l = [];

			Console.WriteLine("Format (spaces can be used instead of comma): X0,Y0;X1,Y1;X2,Y2 etc");
			Console.WriteLine("Press Ctrl+Z (or Ctrl+D on some systems) to mark the end of input");
			string input = MultilineInput().Replace(",", " ");
			string[] parts = input.Split(';', StringSplitOptions.RemoveEmptyEntries);
			foreach (string point in parts) {
				if (point.Trim(null) == "")
					continue;
				string[] coordinates = point.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				try {
					double x = Convert.ToDouble(coordinates[0]);
					double y = Convert.ToDouble(coordinates[1]);
					l.Add(new Point(x, y));
				} catch (Exception) {
					if (l.Count == 0) Console.Error.WriteLine("Invalid input at the beginning");
					else Console.Error.WriteLine($"Invalid input after the point ({l[^1].x}, {l[^1].y})");
					return null;
				}
			}
			return l;
		}
		public void Save()
		{
			try {
				XmlSerializer serializer = new(typeof(List<Point>));
				Console.Write("Enter the file path to write to: ");
				string path = Console.ReadLine() ?? throw new Exception("Invalid path");

				TextWriter writer = new StreamWriter(path);
				serializer.Serialize(writer, vertices);

				writer.Close();
				Console.WriteLine("Polygon saved successfully.");

			} catch (Exception ex) {
				Console.Error.WriteLine("Unable to save the polygon.");
				Console.Error.WriteLine($"Error: {ex.Message}");
			}
			
		}
		public static Polygon? Load()
		{
			Polygon p = new();
			try {
				XmlSerializer serializer = new(typeof(List<Point>));
				Console.Write("Enter the file path to open: ");
				string path = Console.ReadLine() ?? throw new Exception("Invalid path");

				FileStream fs = new(path, FileMode.Open);
                List<Point>? points = (List<Point>?)serializer.Deserialize(fs);
                List<Point> L = points ?? throw new Exception("XML Deserialize returned null");
				p.ParseVArray(L);

				fs.Close();
				Console.WriteLine("Polygon loaded successfully.");
				return p;
            }
            catch (Exception ex) {
				Console.Error.WriteLine("Unable to load the polygon.");
				Console.Error.WriteLine($"Error: {ex.Message}");
				return null;
			}
		}
	}
}