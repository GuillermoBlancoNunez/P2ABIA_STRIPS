public class Predicate
{
    public string Name { get; set; }
    public List<string> Arguments { get; set; }

    public Predicate(string name, params string[] args)
    {
        this.Name = name;
        this.Arguments = args.ToList();
    }

    public override bool Equals(object obj)
    {
        if (obj is Predicate other)
        {
            return this.Name == other.Name && this.Arguments.SequenceEqual(other.Arguments);
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = Name.GetHashCode();
        foreach (string arg in Arguments)
        {
            hash = hash * 23 + arg.GetHashCode();
        }
        return hash;
    }

    public override string ToString()
    {
        return $"{Name}({string.Join(",", Arguments)})";
    }
}

