

public static class Scenario
{
    public static HashSet<Predicate> GetInitialState()
    {
        HashSet<Predicate> state = new HashSet<Predicate>();
        // Bloques encima
        state.Add(new Predicate("Encima", "A", "T1"));
        state.Add(new Predicate("Encima", "B", "A"));
        state.Add(new Predicate("Encima", "C", "B"));
        state.Add(new Predicate("Encima", "D", "T3"));
        state.Add(new Predicate("Encima", "E", "T2"));
        state.Add(new Predicate("Encima", "F", "E"));
        // Bloques libres
        state.Add(new Predicate("Libre", "C"));
        state.Add(new Predicate("Libre", "F"));
        state.Add(new Predicate("Libre", "D"));
        return state;
    }

    public static HashSet<Predicate> GetGoalState()
    {
        HashSet<Predicate> goal = new HashSet<Predicate>();

        goal.Add(new Predicate("Encima", "A", "T3"));
        goal.Add(new Predicate("Encima", "B", "T2"));
        goal.Add(new Predicate("Encima", "C", "T1"));
        goal.Add(new Predicate("Encima", "D", "A"));
        goal.Add(new Predicate("Encima", "E", "C"));
        goal.Add(new Predicate("Encima", "F", "B"));
        return goal;
    }
}

