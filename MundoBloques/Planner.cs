// Planificador que utiliza búsqueda en anchura (BFS) para generar un plan con STRIPS.
public class Planner
{
    public List<MoveAction> GetPlan(WorldState initial, WorldState goal)
    {
        Queue<Tuple<WorldState, List<MoveAction>>> queue = new Queue<Tuple<WorldState, List<MoveAction>>>();
        HashSet<string> visited = new HashSet<string>();

        Tuple<WorldState, List<MoveAction>> initialTuple = new Tuple<WorldState, List<MoveAction>>(initial, new List<MoveAction>());
        queue.Enqueue(initialTuple);
        visited.Add(initial.ToStringRepresentation());

        while (queue.Count > 0)
        {
            Tuple<WorldState, List<MoveAction>> current = queue.Dequeue();
            WorldState currentState = current.Item1;
            List<MoveAction> currentPlan = current.Item2;

            if (currentState.Equals(goal))
            {
                return currentPlan;
            }

            List<MoveAction> applicableActions = this.GetApplicableActions(currentState);
            foreach (MoveAction action in applicableActions)
            {
                if (action.IsApplicable(currentState))
                {
                    WorldState nextState = action.Apply(currentState);
                    string rep = nextState.ToStringRepresentation();
                    if (!visited.Contains(rep))
                    {
                        visited.Add(rep);
                        List<MoveAction> newPlan = new List<MoveAction>(currentPlan);
                        newPlan.Add(action);
                        Tuple<WorldState, List<MoveAction>> newTuple = new Tuple<WorldState, List<MoveAction>>(nextState, newPlan);
                        queue.Enqueue(newTuple);
                    }
                }
            }
        }
        return null; // No se encontró un plan.
    }

    private List<MoveAction> GetApplicableActions(WorldState state)
    {
        List<MoveAction> actions = new List<MoveAction>();
        List<string> tablePositions = new List<string>() { "T1", "T2", "T3" };

        foreach (string block in state.Positions.Keys)
        {
            if (state.IsClear(block))
            {
                string currentSupport = state.Positions[block];
                if (!MoveAction.IsTablePosition(currentSupport))
                {
                    foreach (string tp in tablePositions)
                    {
                        bool isFree = true;
                        foreach (string supp in state.Positions.Values)
                        {
                            if (supp == tp)
                            {
                                isFree = false;
                                break;
                            }
                        }
                        if (isFree)
                        {
                            actions.Add(new MoveAction(block, currentSupport, tp));
                        }
                    }
                }
                foreach (string other in state.Positions.Keys)
                {
                    if (other != block && state.IsClear(other) && other != currentSupport)
                    {
                        actions.Add(new MoveAction(block, currentSupport, other));
                    }
                }
            }
        }
        return actions;
    }
}
