 class Program
    {
        static void Main(string[] args)
        {
            // Inicialización del estado inicial y del objetivo.
            HashSet<Predicate> initialState = Scenario.GetInitialState();
            HashSet<Predicate> goalState = Scenario.GetGoalState();

            // Se crea el agente que calcula el plan basado en el estado inicial y objetivo.
            Agent agent = new Agent(initialState, goalState);

            // Mostrar el estado inicial en la terminal.
            Console.WriteLine("Estado Inicial:");
            PrintState(agent.CurrentState);
            PrintVisualState(agent.CurrentState);

            // Mostrar el plan completo, si se encontró.
            if (agent.Plan != null)
            {
                Console.WriteLine("\nPlan de Acciones:");
                int index = 1;
                foreach (var action in agent.Plan)
                {
                    Console.WriteLine($"{index++}: {action}");
                }
            }
            else
            {
                Console.WriteLine("No se encontró un plan.");
                return;
            }

            // Ejecución paso a paso del plan.
            Console.WriteLine("\nPresiona Enter para ejecutar cada acción...");
            foreach (var action in agent.Plan)
            {
                Console.ReadLine();
                Console.WriteLine($"\nEjecutando acción: {action}");
                agent.CurrentState = action.Apply(agent.CurrentState);
                PrintState(agent.CurrentState);
                PrintVisualState(agent.CurrentState);
            }

            Console.WriteLine("\nPlan completado. Presiona Enter para salir.");
            Console.ReadLine();
        }

        // Función auxiliar para imprimir el estado (lista de predicados) en la terminal.
        static void PrintState(HashSet<Predicate> state)
        {
            Console.WriteLine("Estado actual:");
            foreach (var predicate in state)
            {
                Console.WriteLine(predicate);
            }
            Console.WriteLine(new string('-', 30));
        }

        // Función para mostrar una representación visual de las pilas en las posiciones de la mesa.
        static void PrintVisualState(HashSet<Predicate> state)
        {
            // Definimos las posiciones de la mesa.
            string[] tablePositions = { "T1", "T2", "T3" };
            // Diccionario para almacenar las pilas de cada posición.
            Dictionary<string, List<string>> stacks = new Dictionary<string, List<string>>();
            foreach (string tp in tablePositions)
            {
                stacks[tp] = new List<string>();
            }

            // Para cada posición, buscamos el bloque que está directamente sobre la mesa
            // y seguimos la cadena "Encima" para obtener la pila.
            foreach (string tp in tablePositions)
            {
                var baseBlock = state.FirstOrDefault(p => p.Name == "Encima" && p.Arguments[1] == tp)?.Arguments[0];
                if (baseBlock != null)
                {
                    stacks[tp].Add(baseBlock);
                    string current = baseBlock;
                    bool found = true;
                    while (found)
                    {
                        found = false;
                        var next = state.FirstOrDefault(p => p.Name == "Encima" && p.Arguments[1] == current)?.Arguments[0];
                        if (next != null)
                        {
                            stacks[tp].Add(next);
                            current = next;
                            found = true;
                        }
                    }
                }
            }

            // Imprime la representación visual:
            Console.WriteLine("Representación visual del estado:");
            foreach (string tp in tablePositions)
            {
                Console.Write(tp + ": ");
                if (stacks[tp].Count == 0)
                {
                    Console.WriteLine("[ ]");
                }
                else
                {
                    // Se muestra la pila de abajo hacia arriba.
                    foreach (var block in stacks[tp])
                    {
                        Console.Write($"[{block}] ");
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine(new string('-', 30));
        }
    }
