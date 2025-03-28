/// <summary>
/// Represents a STRIPS action in the blocks world domain.
/// </summary>
public class StripsAction
{
    /// <summary>
    /// Gets or sets the name of the action.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the set of preconditions required for the action.
    /// </summary>
    public HashSet<Predicate> Preconds { get; set; }

    /// <summary>
    /// Gets or sets the set of effects that are added when the action is executed.
    /// </summary>
    public HashSet<Predicate> AddEffects { get; set; }

    /// <summary>
    /// Gets or sets the set of effects that are removed when the action is executed.
    /// </summary>
    public HashSet<Predicate> DelEffects { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StripsAction"/> class.
    /// </summary>
    /// <param name="name">The name of the action.</param>
    /// <param name="preconds">The preconditions for the action.</param>
    /// <param name="addEffects">The effects added by the action.</param>
    /// <param name="delEffects">The effects removed by the action.</param>
    public StripsAction(string name, IEnumerable<Predicate> preconds, IEnumerable<Predicate> addEffects, IEnumerable<Predicate> delEffects)
    {
        this.Name = name;
        this.Preconds = new HashSet<Predicate>(preconds);
        this.AddEffects = new HashSet<Predicate>(addEffects);
        this.DelEffects = new HashSet<Predicate>(delEffects);
    }

    /// <summary>
    /// Determines whether the action is applicable in the given state.
    /// </summary>
    /// <param name="state">The current state as a set of predicates.</param>
    /// <returns>
    ///   <c>true</c> if all preconditions are satisfied in the state; otherwise, <c>false</c>.
    /// </returns>
    public bool IsApplicable(HashSet<Predicate> state)
    {
        return this.Preconds.IsSubsetOf(state);
    }

    /// <summary>
    /// Applies the action to the given state and returns the new state.
    /// </summary>
    /// <param name="state">The current state as a set of predicates.</param>
    /// <returns>A new state resulting from applying the action.</returns>
    public HashSet<Predicate> Apply(HashSet<Predicate> state)
    {
        HashSet<Predicate> newState = new HashSet<Predicate>(state);
        newState.ExceptWith(this.DelEffects);
        newState.UnionWith(this.AddEffects);
        return newState;
    }

    /// <summary>
    /// Returns a string that represents the current action.
    /// </summary>
    /// <returns>A string that represents the current action.</returns>
    public override string ToString()
    {
        return this.Name;
    }
}
