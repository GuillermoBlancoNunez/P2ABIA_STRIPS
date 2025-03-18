class Program
{
    static void Main()
    {
        Estado estadoInicial = new Estado(new HashSet<string>
        {
            "On(A, B)", "On(B, C)", "OnTable(C)", "Clear(A)", "ArmEmpty()"
        });

        Estado estadoObjetivo = new Estado(new HashSet<string>
        {
            "On(B, A)", "On(A, C)", "OnTable(C)", "Clear(B)", "ArmEmpty()"
        });

        List<Accion> acciones = new List<Accion>
        {
            new Accion("PickUp(A)", new List<string> { "OnTable(A)", "Clear(A)", "ArmEmpty()" },
                                        new List<string> { "OnTable(A)", "ArmEmpty()" },
                                        new List<string> { "Holding(A)" }),

            new Accion("PutDown(A)", new List<string> { "Holding(A)" },
                                       new List<string> { "Holding(A)" },
                                       new List<string> { "OnTable(A)", "ArmEmpty()" }),

            new Accion("Stack(A, B)", new List<string> { "Holding(A)", "Clear(B)" },
                                        new List<string> { "Holding(A)" },
                                        new List<string> { "On(A, B)", "Clear(A)", "ArmEmpty()" }),

            new Accion("Unstack(A, B)", new List<string> { "On(A, B)", "Clear(A)", "ArmEmpty()" },
                                          new List<string> { "On(A, B)", "Clear(A)", "ArmEmpty()" },
                                          new List<string> { "Holding(A)", "Clear(B)" }),

            new Accion("PickUp(B)", new List<string> { "OnTable(B)", "Clear(B)", "ArmEmpty()" },
                                        new List<string> { "OnTable(B)", "ArmEmpty()" },
                                        new List<string> { "Holding(B)" }),

            new Accion("PutDown(B)", new List<string> { "Holding(B)" },
                                       new List<string> { "Holding(B)" },
                                       new List<string> { "OnTable(B)", "ArmEmpty()" }),

            new Accion("Stack(B, A)", new List<string> { "Holding(B)", "Clear(A)" },
                                        new List<string> { "Holding(B)" },
                                        new List<string> { "On(B, A)", "Clear(B)", "ArmEmpty()" })
        };

        List<Accion> plan = PlanificadorSTRIPS.GenerarPlan(estadoInicial, estadoObjetivo, acciones);

        if (plan != null)
        {
            Console.WriteLine("Plan encontrado:");
            foreach (Accion accion in plan)
            {
                Console.WriteLine(accion);
            }
        }
    }
}
