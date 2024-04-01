using System.Diagnostics.Contracts;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace Program
{
	public partial class Polygon
	{
		List<Point> vertices = [];
        HashSet<Point> vset = [];
		Dictionary<Point, int> repeatCount = [];
		List<Vector> edges = [];

		private void ParseVArray(List<Point> vertices)
		{
			this.vertices = [];
			vset = [];
			repeatCount = [];
			edges = [];
			AddVertex(vertices, -1);
		}
		public Polygon(List<Point> vertices) => ParseVArray(vertices);
		public Polygon() {}
		public Polygon(params Point[] vertices) => ParseVArray([.. vertices]);
		public bool SwapPoints(int i1, int i2)
		{
			try {
                (vertices[i2], vertices[i1]) = (vertices[i1], vertices[i2]);
				edges = [];
                return true;
			} catch(Exception) { return false; }
		}
		public bool AddVertexUnique(List<Point> A, int i)
		{
			List<Point> l = [];
			foreach (Point p in A)
				if (!vset.Contains(p))
					l.Add(p);
			return AddVertex(l, i);
		}
		public bool AddVertex(List<Point> A, int i)
		{
			if (i == -1)
				i = vertices.Count;
			try {
				vertices.InsertRange(i, A);
				foreach (Point p in A) {
					if (!vset.Add(p))
						repeatCount.Add(p, 1);
				}
				edges = [];
				return true;
			} catch(Exception) { return false; }
        }
		public bool RemoveVertex(Point A)
		{
			if (!vset.Contains(A))
				return false;
			
			if (repeatCount.TryGetValue(A, out int value)) {
				repeatCount[A] = --value;
				if (value == 0)
					repeatCount.Remove(A);
			}
			else
				vset.Remove(A);
			for (int i = vertices.Count - 1; i >= 0; i--)
				if (vertices[i] == A) {
					vertices.RemoveAt(i);
					break;
				}
			edges = [];
			return true;
		}
		public double Surface()
		{
			if (!IsSimple())
				return -1;
			double primary = 0, secondary = 0;
			for (int i = 0; i < vertices.Count - 1; i++) {
				primary += vertices[i].x * vertices[i + 1].y;
				secondary += vertices[i].y * vertices[i + 1].x;
			}
			primary += vertices[^1].x * vertices[0].y;
			secondary += vertices[^1].y * vertices[0].x;
			return Math.Abs(primary - secondary) / 2;
		}
		public double Circumference()
		{
			if (!FormEdges())
				return 0;
			double length = 0;
			foreach (Vector v in edges)
				length += v.Length();

			return length;
		}
		public bool IsConvex()
		{
			if (!IsSimple() || !FormEdges())
				return false;
			int k = 0;
			for (int i = 0; i < edges.Count - 1; i++) {
				if (Vector.VectorM(edges[i], edges[i+1]) * k < 0)
					return false;
				if (k == 0)
					k = Math.Sign(Vector.VectorM(edges[i], edges[i+1]));
			}
			if (Vector.VectorM(edges[^1], edges[0]) * k < 0)
				return false;
			return true;
		}
		static private bool Intersect(Vector A, Vector B) => (!A.OSS(B.A, B.B) && !B.OSS(A.A, A.B));
		private bool FormEdges()
		{
			if (edges.Count != 0)
				return true;
			if (vertices.Count < 2)
				return false;
			for (int i = 0; i < vertices.Count-1; i++)
				edges.Add(new Vector(vertices[i], vertices[i+1]));
			if (vertices.Count > 2)
				edges.Add(new Vector(vertices[^1], vertices[0]));
			return true;
		}
		public bool IsSimple()
		{
			// Domaci 6 - proveri da li je prost (nema ponovljenih Vertices i stranice se ne seku)
			if (repeatCount.Count > 0 && vertices.Count > 0)
				return false;

			FormEdges();
			for (int i = 0; i < edges.Count; i++) {
				int k = 0;
				for (int p = 0; p < edges.Count; p++)
					if (Intersect(edges[i], edges[p]))
						k++;
				if (k != 3)
					return false;
			}
			
			return true;
		}
		public Polygon ConvexHull()
		{
			// Domaci 7 - formiraj konveksni omotac
			Polygon p = new(), n = new();
			if (!IsSimple())
				return p;
			if(!FormEdges())
				return new Polygon(vertices);
			int positive = 0, negative = 0;

			for (int i = 0; i < edges.Count - 1; i++) {
				if (Vector.VectorM(edges[i], edges[i+1]) < 0) {
					negative++;
					n.AddVertex([edges[i].B], -1);
				}
				else {
					positive++;
					p.AddVertex([edges[i].B], -1);
				}
			}
			if (Vector.VectorM(edges[^1], edges[0]) < 0) {
				negative++;
				n.AddVertex([edges[^1].B], -1);
			}
			else {
				positive++;
				p.AddVertex([edges[^1].B], -1);
			}
			
			if (positive > negative)
				return p;
			else
				return n;
		}
		
	}
}