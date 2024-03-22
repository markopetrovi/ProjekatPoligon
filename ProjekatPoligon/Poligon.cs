using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Poligon
{
	List<Tacka> Temena;

	public Poligon(List<Tacka> Niz_temena) => Temena = Niz_temena;
	public Poligon() => Temena = new List<Tacka>();
	public Poligon(params Tacka[] Temena) => this.Temena = new List<Tacka>(Temena);
	public void DodajTeme(Tacka A) => Temena.Add(A);
	public double Povrsina()
	{
		if (!Prost())
			return -1;
		double glavna = 0, sporedna = 0;
		for (int i = 0; i < Temena.Count - 1; i++) {
			glavna += Temena[i].x * Temena[i + 1].y;
			sporedna += Temena[i].y * Temena[i + 1].x;
		}
		glavna += Temena[Temena.Count - 1].x * Temena[0].y;
		sporedna += Temena[Temena.Count - 1].y * Temena[0].x;
		return Math.Abs(glavna - sporedna) / 2;
	}
	public double Obim()
	{
		if (Temena.Count < 2) return 0;
		Tacka prev = Temena[0];
		double duzina = 0;
		for (int i = 1; i < Temena.Count; i++) {
			duzina += new Vektor(prev, Temena[i]).Duzina();
			prev = Temena[i];
		}
		return duzina;
	}
	public void Stampaj()
	{
		for (int i = 0; i < Temena.Count; i++)
			Console.WriteLine($"A{i}: ({Temena[i].x}, {Temena[i].y})");
	}
	public bool Konveksan()
	{
		if (!Prost())
			return false;
		List<Tacka> l = new List<Tacka>();
		foreach (Tacka T in Temena) { l.Add(T); }
		l.Add(l[0]);
		int k = 0;
		for (int i = 0; i < Temena.Count - 1; i++) {
			Vektor A = new Vektor(l[i], l[i + 1]);
			Vektor B = new Vektor(l[i + 1], l[i + 2]);
			if (Vektor.Vektorski(A, B) * k < 0)
				return false;
			if (k == 0)
				k = Math.Sign(Vektor.Vektorski(A, B));
		}
		return true;
	}
	public bool Prost()
	{
		// Domaci 6 - proveri da li je prost (nema ponovljenih temena i stranice se ne seku)
		if (Temena.Count < 2)
			return false;
		HashSet<Tacka> set = new HashSet<Tacka>();
		foreach(Tacka tacka in Temena) {
			if (set.Contains(tacka))
				return false;
			set.Add(tacka);
		}
		if (Temena.Count == 3)
			return Vektor.Vektorski(new Vektor(Temena[0], Temena[1]), new Vektor(Temena[0], Temena[2])) != 0;

		Tacka prev = Temena[0];
		for (int i = 1; i < Temena.Count; i++) {
			Tacka previous = Temena[i+1];
			Vektor osnova = new Vektor(prev, Temena[i]);
			for (int p = i+2; p < Temena.Count; p++) {
				if (osnova.SIS(previous, Temena[p]))
					return false;
				previous = Temena[p];
			}
			prev = Temena[i];
		}
		
		return true;
	}
	public Poligon Omotac()
	{
		// Domaci 7 - formiraj konveksni omotac
		return new Poligon();
	}
	public void Snimi()
	{
		try {
			BinaryFormatter formatter = new BinaryFormatter();
			Console.Write("Enter the file name: ");
			using (FileStream stream = new FileStream(Console.ReadLine(), FileMode.Create)) {
				formatter.Serialize(stream, Temena);
			}
		} catch (Exception ex) {
			Console.Error.WriteLine("Unable to save the polygon.");
			Console.Error.WriteLine($"Error: {ex.Message}");
		}
		
	}
	public void Ucitaj()
	{
		try {
			BinaryFormatter formatter = new BinaryFormatter();
			Console.Write("Enter the file name: ");
			using (FileStream stream = new FileStream(Console.ReadLine(), FileMode.Open)) {
				Temena = (List<Tacka>)formatter.Deserialize(stream);
			}
		} catch (Exception ex) {
			Console.Error.WriteLine("Unable to load the polygon.");
			Console.Error.WriteLine($"Error: {ex.Message}");
		}
	}
}
