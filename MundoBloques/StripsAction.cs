public class StripsAction
{
    public string Name { get; set; }
    public HashSet<Predicate> Preconds { get; set; }
    public HashSet<Predicate> AddEffects { get; set; }
    public HashSet<Predicate> DelEffects { get; set; }

    public StripsAction(string name, IEnumerable<Predicate> preconds, IEnumerable<Predicate> addEffects, IEnumerable<Predicate> delEffects)
    {
        this.Name = name;
        this.Preconds = new HashSet<Predicate>(preconds);
        this.AddEffects = new HashSet<Predicate>(addEffects);
        this.DelEffects = new HashSet<Predicate>(delEffects);
    }

    public bool IsApplicable(HashSet<Predicate> state)
    {
        return this.Preconds.IsSubsetOf(state);
    }

    public HashSet<Predicate> Apply(HashSet<Predicate> state)
    {
        HashSet<Predicate> newState = new HashSet<Predicate>(state);
        newState.ExceptWith(this.DelEffects);
        newState.UnionWith(this.AddEffects);
        return newState;
    }

    public override string ToString()
    {
        return this.Name;
    }
}

