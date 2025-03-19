using System;
using System.Collections.Generic;
using System.Linq;

public class StripsPlanner
{
    public List<StripsAction> GetPlan(HashSet<Predicate> initial, HashSet<Predicate> goal)
    {
        Queue<Tuple<HashSet<Predicate>, List<StripsAction>>> queue = new Queue<Tuple<HashSet<Predicate>, List<StripsAction>>>();
        HashSet<string> visited = new HashSet<string>();

        queue.Enqueue(new Tuple<HashSet<Predicate>, List<StripsAction>>(initial, new List<StripsAction>()));
        visited.Add(StateToString(initial));

        List<StripsAction> allActions = StripsDomain.GetAllActions();

        while (queue.Count > 0)
        {
            var currentTuple = queue.Dequeue();
            HashSet<Predicate> currentState = currentTuple.Item1;
            List<StripsAction> currentPlan = currentTuple.Item2;

            if (goal.IsSubsetOf(currentState))
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
                        queue.Enqueue(new Tuple<HashSet<Predicate>, List<StripsAction>>(nextState, newPlan));
                    }
                }
            }
        }
        return null;
    }

    private string StateToString(HashSet<Predicate> state)
    {
        var sorted = state.Select(p => p.ToString()).OrderBy(s => s);
        return string.Join(";", sorted);
    }
}
