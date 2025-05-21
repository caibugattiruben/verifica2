using System;
using System.IO.Pipes;

List<string> LeggiFile(string path)
{
    List<string> lista = new List<string>();
    StreamReader r = new StreamReader(path);
    string riga=r.ReadLine();
    riga = r.ReadLine();
    while(riga != null)
    {
      lista.Add(riga);
      riga = r.ReadLine();
    }
    return lista;
}
List<string> FiltraPerTempo(List<string> lista)
{
    List<string> veloci = new List<string>();
    foreach(string s in lista)
    {
        string[] persona = s.Split(';');
        if (int.Parse(persona[4]) <= 60)
        {
            veloci.Add(persona[0]+" " + persona[1]+" - " + persona[3]+" (" + persona[4]+" minuti)");
        }
    }
    return veloci;
}
void CalcolaStatistiche(List<string> lista)
{
    float media = 0;
    string lento = lista[0].Split(';')[0]+" " + lista[0].Split(';')[1];
    int tMin = int.Parse(lista[0].Split(';')[4]),msettanta=0;
    foreach (string s in lista)
    {
        string[] persona = s.Split(';');
        media += float.Parse(persona[4]);
        if (tMin <= int.Parse(persona[4]))
        {
            tMin=int.Parse(persona[4]);
            lento = persona[0] + " " + persona[1];
        }
        if (int.Parse(persona[4]) < 70)
        {
            msettanta++;
        }
    }
    media = media / lista.Count();
    Console.WriteLine("Il tempo medio di completamento è " + media);
    Console.WriteLine("Il numero totale di partecipanti è " + lista.Count());
    Console.WriteLine("Il partecipante più lento è stato " + lento + " con un tempo di " + tMin);
    Console.WriteLine("Il numero di partecipanti sotto a 70 minuti sono: " + msettanta);
}
List<string> CercaPerScuola(List<string> lista)
{
    List<string> partecipanti=new List<string>();
    List<string> famiglie = new List<string>();
    Console.WriteLine("Dimmi il nome della scuola ricercata");
    string scuola = Console.ReadLine().ToLower();
    foreach(string s in lista)
    {
        string[] persona = s.Split(';');
        if (persona[3].ToLower() == scuola)
        {
            partecipanti.Add(persona[0]+" "+persona[1]);
            if (!famiglie.Contains(persona[1]))
            {
                famiglie.Add(persona[1]);
            }
        }
    }
    string path = "PartecipantiScuola.txt";
    StreamWriter w = new StreamWriter(path);
    foreach(string s in famiglie)
    {
        w.WriteLine(s);
    }
    w.Close();
    return partecipanti;
}
List<string> CalcolaPodio(List<string> lista)
{
    List<string> podio = new List<string>();
    int min = int.Parse(lista[0].Split(';')[4]);
    string minP="",scuola="";
    foreach(string s in lista)
    {
        string[] persona = s.Split(';');
        if (min >= int.Parse(persona[4]))
        {
            min = int.Parse(persona[4]);
            minP = persona[0] + " " + persona[1];
            scuola=persona[3];
        }
    }
    podio.Add("1 - " + minP +" - "+scuola+" ("+min+" minuti)");

    int min2 =1000000;
    foreach (string s in lista)
    {
        string[] persona = s.Split(';');
        if (min2 > min && int.Parse(persona[4]) < min2)
        {
            min2 = int.Parse(persona[4]);
            minP=persona[0]+" " + persona[1];
            scuola=persona[3];
        }

    }
    podio.Add("2 - " + minP + " - " + scuola + " (" + min2 + " minuti)");

    int min3 = 10000;
    foreach (string s in lista)
    {
        string[] persona = s.Split(';');
        if (min3 > min2 && int.Parse(persona[4]) < min3)
        {
            min2 = int.Parse(persona[4]);
            minP = persona[0] + " " + persona[1];
            scuola = persona[3];
        }

    }
    podio.Add("3 - " + minP + " - " + scuola + " (" + min3 + " minuti)");
    return podio;
}
List <string> TempoMedioPerScuola(List<string> lista)
{
    List<string> scuola= new List<string>();
    foreach(string a in lista)
    {
        string[] persona = a.Split(';');
        if (!scuola.Contains(persona[3]))
        {
            scuola.Add(persona[3]);
        }
    }
    int[] partecipanti=new int[scuola.Count];
    float[] tempo=new float[scuola.Count];
    foreach(string a in lista)
    {
        string[] persona = a.Split(';');
        partecipanti[scuola.IndexOf(persona[3])]++;
        tempo[scuola.IndexOf(persona[3])] += float.Parse(persona[4]);
    }
    for(int i = 0; i < tempo.Length; i++)
    {
        tempo[i] /= partecipanti[i];
    }
    List<string> Tscuole=new List<string>();
    foreach(string s in scuola)
    {
        Tscuole.Add(s+": " + partecipanti[scuola.IndexOf(s)]+" partecipanti - tempo medio " + Math.Round(tempo[scuola.IndexOf(s)],2)+" minuti");
    }
    return Tscuole;
}
string path = "maratona.csv";
List<string> risultati=LeggiFile(path);
List<string> veloci = FiltraPerTempo(risultati);
foreach(string a in veloci)
{
    Console.WriteLine(a);
}
CalcolaStatistiche(risultati);
List<string> partecipanti=CercaPerScuola(risultati);
Console.WriteLine("I nomi degli alunni della scuola cercata sono:");
foreach(string s in partecipanti)
{
    Console.WriteLine(s);
}
List<string> podio = CalcolaPodio(risultati);
Console.WriteLine("Il podio di questa gara è:");
foreach (string s in podio)
{
    Console.WriteLine(s);
}
List<string> Tscuole = TempoMedioPerScuola(risultati);
foreach (string s in Tscuole)
{
    Console.WriteLine(s);
}