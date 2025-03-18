using System;
using System.Collections.Generic;

class Estado
{
    public HashSet<string> Hechos;

    public Estado()
    {
        Hechos = new HashSet<string>();
    }

    public Estado(HashSet<string> hechos)
    {
        Hechos = new HashSet<string>(hechos);
    }

    public bool Contiene(string hecho)
    {
        return Hechos.Contains(hecho);
    }

    public void Agregar(string hecho)
    {
        Hechos.Add(hecho);
    }

    public void Eliminar(string hecho)
    {
        Hechos.Remove(hecho);
    }

    public Estado Clonar()
    {
        return new Estado(new HashSet<string>(Hechos));
    }

    public override string ToString()
    {
        return string.Join(", ", Hechos);
    }
}
