/// <summary>
/// Main entry point for the Blocks World STRIPS application.
/// This application initializes the scenario, prints the initial state and visual representation,
/// displays the computed plan, and then executes the plan step-by-step.
/// </summary>
class Program
{
    /// <summary>
    /// Main method that starts the application.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    static void Main(string[] args)
    {
        HashSet<Predicate> initialState = Scenario.GetInitialState();
        HashSet<Predicate> goalState = Scenario.GetGoalState();

        Agent agent = new Agent(initialState, goalState);

        PrintState(agent.CurrentState, "inicial");
        PrintVisualState(agent.CurrentState);

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

        Console.WriteLine("\nPresiona Enter para ejecutar cada acción...");
        foreach (var action in agent.Plan)
        {
            Console.ReadLine();
            Console.WriteLine($"\nEjecutando acción: {action}");
            agent.CurrentState = action.Apply(agent.CurrentState);
            PrintState(agent.CurrentState, "actual");
            PrintVisualState(agent.CurrentState);
        }

        Console.WriteLine("\nPlan completado. Presiona Enter para salir.");
        Console.ReadLine();
    }

    /// <summary>
    /// Prints the state's predicates to the console.
    /// </summary>
    /// <param name="state">The state as a set of predicates.</param>
    /// <param name="label">A label indicating the type of state (e.g., "inicial", "actual").</param>
    static void PrintState(HashSet<Predicate> state, string label)
    {
        Console.WriteLine(new string('-', 30));
        Console.WriteLine($"Estado {label}:");
        Console.WriteLine(new string('-', 30));
        foreach (var predicate in state)
        {
            Console.WriteLine(predicate);
        }
        Console.WriteLine(new string('-', 30));
    }

    /// <summary>
    /// Displays a visual representation of the towers by identifying bases (predicates with support "Table")
    /// and then following the "Encima" chain upward.
    /// </summary>
    /// <param name="state">The state as a set of predicates.</param>
    static void PrintVisualState(HashSet<Predicate> state)
    {
        var basePredicates = state.Where(p => p.Name == "Encima" && p.Arguments[1] == "Table").ToList();
        List<List<string>> towers = new List<List<string>>();

        foreach (var bp in basePredicates)
        {
            List<string> tower = new List<string>();
            string currentBlock = bp.Arguments[0];
            tower.Add(currentBlock);

            bool found = true;
            while (found)
            {
                var nextPredicate = state.FirstOrDefault(p => p.Name == "Encima" && p.Arguments[1] == currentBlock);
                if (nextPredicate != null)
                {
                    string nextBlock = nextPredicate.Arguments[0];
                    tower.Add(nextBlock);
                    currentBlock = nextBlock;
                }
                else
                {
                    found = false;
                }
            }
            towers.Add(tower);
        }

        Console.WriteLine("Representación visual del estado:");
        Console.WriteLine(new string('-', 30));
        if (towers.Count == 0)
        {
            Console.WriteLine("[No hay torres]");
        }
        else
        {
            int towerIndex = 1;
            foreach (var tower in towers)
            {
                Console.Write($"Torre {towerIndex++}: ");
                foreach (var block in tower)
                {
                    Console.Write($"[{block}] ");
                }
                Console.WriteLine();
            }
        }
        Console.WriteLine(new string('-', 30));
    }
}
