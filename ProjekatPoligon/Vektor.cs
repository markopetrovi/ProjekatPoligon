namespace Program
{
	public class Point
    {
		public double x, y;
		public Point(double x, double y)
		{
			this.x = x;
			this.y = y;
		}
		public Point() {}

        public override bool Equals(object? obj)
        {
            if (obj is not Point p)
                return false;
            if (p.x == x && p.y == y)
				return true;
			return false;
        }
		public static bool operator ==(Point A, Point B) => A.Equals(B);
		public static bool operator !=(Point A, Point B) => A.Equals(B);
        public override int GetHashCode() => HashCode.Combine(x, y);
    }
    public class Vector(Point A, Point B)
	{
		public double x = B.x - A.x, y = B.y - A.y;
		public Point A = A, B = B;

		public static double ScalarM(Vector A, Vector B) => A.x * B.x + A.y * B.y;
		public static double VectorM(Vector A, Vector B) => A.x * B.y - A.y * B.x;
		public bool OSS(Point A, Point B) => (VectorM(this, new Vector(this.B, A)) * VectorM(this, new Vector(this.B, B))) > 0;
		public double Length() => Math.Sqrt(x*x + y*y);
	}
}