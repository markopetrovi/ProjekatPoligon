using System.Text.RegularExpressions;
namespace Program
{
    partial class Program
	{
		static void PrintMenu(bool isNull)
		{
			Console.WriteLine("1) Enter the new polygon");
			Console.WriteLine("2) Load the polygon from file");
			if (!isNull) {
				Console.WriteLine("3) Save the polygon to file");
				Console.WriteLine("4) Add new vertices");
				Console.WriteLine("5) Remove existing vertices");
				Console.WriteLine("6) Print surface area");
				Console.WriteLine("7) Print circumference");
				Console.WriteLine("8) Is the polygon convex?");
				Console.WriteLine("9) Is the polygon simple?");
				Console.WriteLine("10) Print vertices of the polygon's complex hull");
				Console.WriteLine("11) Print the polygon");
				Console.WriteLine("12) Swap two points");
			}
			Console.WriteLine("0) Exit");
			if (!isNull)
				Console.Write("\n\nChoose the option [0-11]: ");
			else
				Console.Write("\n\nChoose the option [0-2]: ");
		}
		static void Main(string[] args)
		{
			int option = -1;
			Polygon? p = null;
			string? optline = null;

			while (option != 0) {
				PrintMenu(p == null);
				while (optline == null || optline == "")
					optline = Console.ReadLine();
				try { option = Convert.ToInt32(optline); }
				catch(Exception) { option = -1; }
				optline = null;
				switch (option) {
					case 0:
						break;
					case 1: {	// Enter the new polygon
						Console.WriteLine("Enter polygon vertices...");
						var l = Polygon.Input();
						if (l == null)
							Console.WriteLine("Failed to create the polygon.");
						else {
							p = new(l);
							Console.WriteLine("Polygon successfully created.");
						}
						break;
					}
					case 2: {	// Load the polygon from file
						Polygon? t = Polygon.Load();
						if (t != null)
							p = t;
						break;
					}
					case 3 when p != null: {	// Save the polygon to file
						p.Save();
						break;
					}
					case 4 when p != null: {	// Add new vertices
						Console.WriteLine("Enter vertices to add...");
						var l = Polygon.Input();
						if (l == null)
							Console.Error.WriteLine("No action made");
						else {
							Console.WriteLine("Enter the starting index where to insert the new vertices \n(-1 for the end of list)");
							int i;
							try { i = Convert.ToInt32(Console.ReadLine()); }
							catch(Exception) { Console.Error.WriteLine("Error: Invalid index"); break;}
							if (!p.AddVertex(l, i)) {
								Console.Error.WriteLine("Error: Index out of range");
								break;
							}
							Console.WriteLine("All vertices successfully added.");
						}
						break;
					}
					case 5 when p != null: {	// Remove existing vertices
						Console.WriteLine("Enter vertices to remove...");
						var l = Polygon.Input();
						if (l == null) {
							Console.Error.WriteLine("No action made");
							break;
						}
						
						bool err = false;
						foreach (Point po in l)
							if (!p.RemoveVertex(po)) {
								err = true;
								Console.Error.WriteLine($"Error removing vertex ({po.x}, {po.y}): no such vertex");
							}
						if (err)
							Console.Error.WriteLine("Operation completed with errors.");
						else
							Console.WriteLine("All vertices successfully removed.");
						break;
					}
					case 6 when p != null: {	// Print surface area
						Console.WriteLine($"Surface area: {p.Surface()}");
						break;
					}
					case 7 when p != null: {	// Print circumference
						Console.WriteLine($"Circumference: {p.Circumference()}");
						break;
					}
					case 8 when p != null: {	// Is the polygon convex?
						if (p.IsConvex())
							Console.WriteLine("This polygon is convex.");
						else
							Console.WriteLine("This polygon is NOT convex.");
						break;
					}
					case 9 when p != null: {	// Is the polygon simple?
						if (p.IsSimple())
							Console.WriteLine("This polygon is simple.");
						else
							Console.WriteLine("This polygon is NOT simple.");
						break;
					}
					case 10 when p != null: {	// Print vertices of the polygon's complex hull
						Console.WriteLine("Convex hull of this polygon consists of:");
						p.ConvexHull().Print();
						break;
					}
					case 11 when p != null: {	// Print the polygon
						Console.WriteLine("This polygon consists of:");
						p.Print();
						break;
					}
					case 12 when p != null: {	// Swap two points
						p.Print();
						Console.Write("Write which two points to swap [Ax-Ay]:\t");
						string? line = Console.ReadLine();
						if (line == null) {
							Console.Error.WriteLine("Invalid input! Got null string?");
							break;
						}
						Regex pattern = MyRegex();
						MatchCollection match = pattern.Matches(line);
						if (match.Count != 2) {
							Console.Error.WriteLine("Invalid input! Enter exactly two points.");
							break;
						}
						int i1 = Convert.ToInt32(match[0].Value);
						int i2 = Convert.ToInt32(match[1].Value);
						if (!p.SwapPoints(i1, i2)) {
							Console.Error.WriteLine("Invalid input! Index out of range.");
							break;
						}
						Console.WriteLine("Points swapped successfully.");
						break;
					}
					default: {
						Console.Error.WriteLine("Invalid option. No action done.");
						break;
					}
				}
				Console.Write("Press any key to continue...");
				try {
					Console.ReadKey();
				} catch (Exception) {
					Console.Read();
				}
				Console.Clear();
			}
		}

        [GeneratedRegex(@"(?<=(?i)A)\d+")]
        private static partial Regex MyRegex();
    }
}