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
		return 0;
	}
	public double Obim()
	{
		// Domaci 3 - uradi obim
		return 0;
	}
	public void Stampaj()
	{
		// Domaci 4 - stampaj sva temena
	}
	public bool Konveksan()
	{
		// Domaci 5 – Proveri da li je konveksan
		return false;
	}
	public bool Prost()
	{
		// Domaci 6 - proveri da li je prost (nema ponovljenih temena i stranice se ne seku)
		return false;
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
