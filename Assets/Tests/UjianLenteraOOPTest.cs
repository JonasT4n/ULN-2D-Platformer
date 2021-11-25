using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UjianLenteraOOPTest
{
    [Test]
    public void TestingInheritance()
    {
        Gelas gelas = new Gelas(6, 7, "Bunga");
        Debug.Log(gelas.HitungVolume());
    }

    [Test]
    public void TestingPolymorphism()
    {
        IGambar manga = new Manga();
        IGambar peta = new Peta();
        Debug.Log(manga.Gambarkan());
        Debug.Log(peta.Gambarkan());
    }

    [Test]
    public void TestingEncapsulation()
    {
        KTP ktp = new KTP("1234", "30 Maret 2001", "Jln. Kebon Jerok No.94");
        //ktp.NIK = "5678"; // ERROR karena NIK tidak dapat diakses secara langsung
        Debug.Log(ktp.GetNIK());
    }

    [Test]
    public void TestingAbstraction()
    {
        //Hewan h = new Hewan();
        Hewan h = new Kucing();
        Hewan i = new Kuda();
        Debug.Log(h);
        Debug.Log(i);
    }
}

#region Contoh Inheritance
public class Tabung
{
    int tinggi;
    int jariJari;

    public Tabung(int tinggi, int jariJari)
    {
        this.tinggi = tinggi;
        this.jariJari = jariJari;
    }
    
    public float HitungVolume()
    {
        // t * PI * r^2
        return tinggi * Mathf.PI * Mathf.Pow(jariJari, 2f); 
    }
}

public class Gelas : Tabung
{
    string motif;

    public Gelas(int tinggi, int jariJari, string motif) : base(tinggi, jariJari)
    {
        this.motif = motif;
    }

    public string GetMotif() => motif;
}
#endregion

#region Contoh Polymorphism
public interface IGambar
{
    string Gambarkan();
}

public class Komik { }

public class Manga : Komik, IGambar
{
    public string Gambarkan() => "Anime Manga";
}

public class Peta : IGambar
{
    public string Gambarkan() => "Peta";
}
#endregion

#region Contoh Encapsulasi
public class KTP
{
    private string NIK;
    private string tanggalLahir;
    private string alamat;

    public KTP(string NIK, string tanggalLahir, string alamat)
    {
        this.NIK = NIK;
        this.tanggalLahir = tanggalLahir;
        this.alamat = alamat;
    }

    public string GetNIK() => NIK;
    public string GetTglLahir() => tanggalLahir;

    public void SetAlamat(string alamatBaru) => alamat = alamatBaru;
    public string GetAlamat() => alamat;
}
#endregion

#region Contoh Abstraction
public abstract class Hewan
{
    public abstract void KeluarkanSuara();
}

public class Kucing : Hewan
{
    public override void KeluarkanSuara() => Debug.Log("Meowww");
}

public class Kuda : Hewan
{
    public override void KeluarkanSuara() => Debug.Log("HIIIIIIIIIIIIIII HAAAAAAAAAAAAA");
}
#endregion