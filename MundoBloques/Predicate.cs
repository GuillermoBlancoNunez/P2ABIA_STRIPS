/// <summary>
/// Represents a predicate in the blocks world domain.
/// </summary>
public class Predicate
{
    /// <summary>
    /// Gets or sets the name of the predicate.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the list of arguments for the predicate.
    /// </summary>
    public List<string> Arguments { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Predicate"/> class with the specified name and arguments.
    /// </summary>
    /// <param name="name">The name of the predicate.</param>
    /// <param name="args">The arguments of the predicate.</param>
    public Predicate(string name, params string[] args)
    {
        this.Name = name;
        this.Arguments = args.ToList();
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current predicate.
    /// </summary>
    /// <param name="obj">The object to compare with the current predicate.</param>
    /// <returns><c>true</c> if the specified object is equal to the current predicate; otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
        if (obj is Predicate other)
        {
            return this.Name == other.Name && this.Arguments.SequenceEqual(other.Arguments);
        }
        return false;
    }

    /// <summary>
    /// Returns a hash code for the current predicate.
    /// </summary>
    /// <returns>A hash code for the current predicate.</returns>
    public override int GetHashCode()
    {
        int hash = Name.GetHashCode();
        foreach (string arg in Arguments)
        {
            hash = hash * 23 + arg.GetHashCode();
        }
        return hash;
    }

    /// <summary>
    /// Returns a string that represents the current predicate.
    /// </summary>
    /// <returns>A string representation of the predicate.</returns>
    public override string ToString()
    {
        return $"{Name}({string.Join(",", Arguments)})";
    }
}
