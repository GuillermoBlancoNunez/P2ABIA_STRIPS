
public class StripsPlanner
{
    
    private int CostCalculation(List<StripsAction> currentPlan)
    {
        return currentPlan.Count;
    }

    // Calcula la heurística a partir del estado actual y el estado objetivo.
    private int HeuristicCalculation(HashSet<Predicate> State, HashSet<Predicate> goalState)
    {
        int pen = 0;

        // Recorremos cada predicado del objetivo
        foreach (var goalPredicate in goalState)
        {
            if (goalPredicate.Name == "Libre")
                if (!State.Contains(goalPredicate))
                    pen += CountBlocksAbove(goalPredicate.Arguments[0], State);

            if (goalPredicate.Name == "Encima")
                if (FindSupport(goalPredicate.Arguments[0],goalState) == FindSupport(goalPredicate.Arguments[0], State))
                    pen++;                    
        }

        return pen;
    }

    private int CountBlocksAbove(string block, HashSet<Predicate> state)
    {
        // Buscamos el predicado que indique que algún bloque está encima del bloque actual.
        // Es decir, buscamos "Encima(X, block)".
        var predicate = state.FirstOrDefault(p => p.Name == "Encima" && p.Arguments[1] == block);

        // Si no encontramos ningún predicado, no hay bloque encima.
        if (predicate == null)
        {
            return 0;
        }
        else
        {
            // Si encontramos el predicado, obtenemos el bloque que está encima.
            string blockAbove = predicate.Arguments[0];

            // Se cuenta 1 (por el bloque que encontramos) y se llama recursivamente para contar
            // los bloques que están encima del bloque encontrado.
            return 1 + CountBlocksAbove(blockAbove, state);
        }
    }


    private string FindSupport(string block, HashSet<Predicate> state)
    {
        // Buscamos el predicado que indica "Encima(block, support)"
        var predicate = state.FirstOrDefault(p => p.Name == "Encima" && p.Arguments[0] == block);
        
        // Obtenemos el soporte sobre el que está el bloque.
        string support = predicate.Arguments[1];

        // Si el soporte es una posición fija (por ejemplo, T1, T2 o T3),
        // entonces esa es la base.
        if (support == "T1" || support == "T2" || support == "T3")
        {
            return support;
        }
        else
        {
            // Si el soporte es otro bloque, se hace una llamada recursiva para
            // determinar la base de ese bloque.
            return FindSupport(support, state);
        }
    }

    private string StateToString(HashSet<Predicate> state)
    {
        var sorted = state.Select(p => p.ToString()).OrderBy(s => s);
        return string.Join(";", sorted);
    }
        
    public List<StripsAction> GetPlan(HashSet<Predicate> initialState, HashSet<Predicate> goalState)
    {
        PriorityQueue<Tuple<HashSet<Predicate>, List<StripsAction>>, int> Pqueue = new PriorityQueue<Tuple<HashSet<Predicate>, List<StripsAction>>, int>();
        HashSet<string> visited = new HashSet<string>();

        Pqueue.Enqueue(new Tuple<HashSet<Predicate>, List<StripsAction>>(initialState, new List<StripsAction>()), 0);
        visited.Add(StateToString(initialState));

        List<StripsAction> allActions = StripsDomain.GetAllActions();

        while (Pqueue.Count > 0)
        {
            var currentTuple = Pqueue.Dequeue();
            HashSet<Predicate> currentState = currentTuple.Item1;
            List<StripsAction> currentPlan = currentTuple.Item2;

            if (goalState.IsSubsetOf(currentState))
            {
                return currentPlan;
            }

            foreach (var action in allActions)
            {
                if (action.IsApplicable(currentState))
                {
                    HashSet<Predicate> nextState = action.Apply(currentState);
                    string stateStr = StateToString(nextState);
                    if (!visited.Contains(stateStr))
                    {
                        visited.Add(stateStr);
                        List<StripsAction> newPlan = new List<StripsAction>(currentPlan);
                        newPlan.Add(action);
                        int cost = CostCalculation(newPlan);
                        int heuristic = HeuristicCalculation(nextState, goalState);
                        int priority = cost + heuristic;
                        Pqueue.Enqueue(new Tuple<HashSet<Predicate>, List<StripsAction>>(nextState, newPlan), priority);
                    }
                }
            }
        }
        return null;
    }

}
