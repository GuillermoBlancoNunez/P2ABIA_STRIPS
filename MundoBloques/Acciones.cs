class Accion
{
    public string Nombre;
    public List<string> Precondiciones;
    public List<string> EfectosEliminar;
    public List<string> EfectosAgregar;

    public Accion(string nombre, List<string> precondiciones, List<string> efectosEliminar, List<string> efectosAgregar)
    {
        Nombre = nombre;
        Precondiciones = precondiciones;
        EfectosEliminar = efectosEliminar;
        EfectosAgregar = efectosAgregar;
    }

    public bool EsAplicable(Estado estado)
    {
        foreach (string precondicion in Precondiciones)
        {
            if (!estado.Contiene(precondicion))
            {
                return false;
            }
        }
        return true;
    }

    public Estado Aplicar(Estado estado)
    {
        Estado nuevoEstado = estado.Clonar();
        foreach (string hecho in EfectosEliminar)
        {
            nuevoEstado.Eliminar(hecho);
        }
        foreach (string hecho in EfectosAgregar)
        {
            nuevoEstado.Agregar(hecho);
        }
        return nuevoEstado;
    }

    public override string ToString()
    {
        return Nombre;
    }
}
