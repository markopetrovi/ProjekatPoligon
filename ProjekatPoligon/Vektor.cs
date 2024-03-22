using System;

public class Tacka
{
	public double x, y;
	public Tacka(double x, double y)
	{
		this.x = x;
		this.y = y;
	}
}
public class Vektor
{
	public double x, y;
	public Tacka A, B;
	public Vektor(double x, double y)
	{
		this.x = x;
		this.y = y;
	}
	public Vektor(Tacka A, Tacka B)
	{
		this.x = B.x - A.x;
		this.y = B.y - A.y;
		this.A = A;
		this.B = B;
	}
	public static double Skalarni(Vektor A, Vektor B) => A.x * B.x + A.y * B.y;
	public static double Vektorski(Vektor A, Vektor B) => A.x * B.y - A.y * B.x;
	public bool SIS(Tacka A, Tacka B) => (Vektorski(this, new Vektor(this.B, A)) * Vektorski(this, new Vektor(this.B, B))) >= 0;
	public double Duzina() => Math.Sqrt(x*x + y*y);
}
