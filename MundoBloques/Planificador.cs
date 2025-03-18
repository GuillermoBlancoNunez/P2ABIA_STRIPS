class PlanificadorSTRIPS
{
    public static List<Accion> GenerarPlan(Estado inicial, Estado objetivo, List<Accion> acciones)
    {
        List<Accion> plan = new List<Accion>();
        Estado estadoActual = inicial.Clonar();

        while (!objetivo.Hechos.IsSubsetOf(estadoActual.Hechos))
        {
            bool accionEncontrada = false;

            foreach (Accion accion in acciones)
            {
                if (accion.EsAplicable(estadoActual))
                {
                    estadoActual = accion.Aplicar(estadoActual);
                    plan.Add(accion);
                    accionEncontrada = true;
                    break;
                }
            }

            if (!accionEncontrada)
            {
                Console.WriteLine("No se encontró un plan válido.");
                return null;
            }
        }
        return plan;
    }
}
