/// <summary>
/// Implements the STRIPS planner using an A* search algorithm.
/// </summary>
public class StripsPlanner
{
    /// <summary>
    /// Calculates the cost of the current plan as the number of actions.
    /// </summary>
    /// <param name="currentPlan">The list of actions taken so far.</param>
    /// <returns>The cost value.</returns>
    private int CostCalculation(List<StripsAction> currentPlan)
    {
        return currentPlan.Count;
    }

    /// <summary>
    /// Computes the heuristic value by counting how many predicates in the goal state are missing in the current state.
    /// </summary>
    /// <param name="State">The current state represented as a set of predicates.</param>
    /// <param name="goalState">The goal state represented as a set of predicates.</param>
    /// <returns>The heuristic value.</returns>
    private int HeuristicCalculation(HashSet<Predicate> State, HashSet<Predicate> goalState)
    {
        int penalization = 0;
        foreach (var predicate in goalState)
        {
            if (!State.Contains(predicate))
                penalization++;
        }
        return penalization;
    }

    /// <summary>
    /// Converts a state (a set of predicates) into a sorted string representation.
    /// </summary>
    /// <param name="state">The state as a set of predicates.</param>
    /// <returns>A string representation of the state.</returns>
    private string StateToString(HashSet<Predicate> state)
    {
        var sorted = state.Select(p => p.ToString()).OrderBy(s => s);
        return string.Join(";", sorted);
    }

    /// <summary>
    /// Generates a plan that transforms the initial state into the goal state using A* search.
    /// </summary>
    /// <param name="initialState">The initial state as a set of predicates.</param>
    /// <param name="goalState">The goal state as a set of predicates.</param>
    /// <returns>
    /// A list of <see cref="StripsAction"/> representing the plan if one is found; otherwise, null.
    /// </returns>
    public List<StripsAction> GetPlan(HashSet<Predicate> initialState, HashSet<Predicate> goalState)
    {
        PriorityQueue<Tuple<HashSet<Predicate>, List<StripsAction>>, int> Pqueue =
            new PriorityQueue<Tuple<HashSet<Predicate>, List<StripsAction>>, int>();
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
