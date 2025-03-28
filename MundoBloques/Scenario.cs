/// <summary>
/// Provides the initial and goal states for the blocks world scenario.
/// </summary>
public static class Scenario
{
    /// <summary>
    /// Returns the initial state of the blocks world as a set of predicates.
    /// </summary>
    /// <returns>A <see cref="HashSet{Predicate}"/> representing the initial state.</returns>
    public static HashSet<Predicate> GetInitialState()
    {
        HashSet<Predicate> state = new HashSet<Predicate>();
        state.Add(new Predicate("Encima", "A", "Table"));
        state.Add(new Predicate("Encima", "B", "A"));
        state.Add(new Predicate("Encima", "C", "B"));
        state.Add(new Predicate("Encima", "D", "Table"));
        state.Add(new Predicate("Encima", "E", "Table"));
        state.Add(new Predicate("Encima", "F", "E"));
        state.Add(new Predicate("Libre", "C"));
        state.Add(new Predicate("Libre", "F"));
        state.Add(new Predicate("Libre", "D"));
        state.Add(new Predicate("Libre", "Table"));
        return state;
    }

    /// <summary>
    /// Returns the goal state of the blocks world as a set of predicates.
    /// </summary>
    /// <returns>A <see cref="HashSet{Predicate}"/> representing the goal state.</returns>
    public static HashSet<Predicate> GetGoalState()
    {
        HashSet<Predicate> goal = new HashSet<Predicate>();
        goal.Add(new Predicate("Encima", "A", "Table"));
        goal.Add(new Predicate("Encima", "B", "Table"));
        goal.Add(new Predicate("Encima", "C", "Table"));
        goal.Add(new Predicate("Encima", "D", "A"));
        goal.Add(new Predicate("Encima", "E", "C"));
        goal.Add(new Predicate("Encima", "F", "B"));
        return goal;
    }
}
