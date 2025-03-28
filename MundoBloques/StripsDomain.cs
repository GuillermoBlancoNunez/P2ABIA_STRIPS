/// <summary>
/// Represents the domain for the blocks world in STRIPS.
/// This class generates all possible move actions.
/// </summary>
public static class StripsDomain
{
    /// <summary>
    /// Generates all possible actions of the form Mover(b, x, y), which represents moving block <c>b</c> from support <c>x</c> to support <c>y</c>.
    /// x and y are taken from the union of the set of blocks and the table.
    /// </summary>
    /// <returns>A list of <see cref="StripsAction"/> representing possible moves.</returns>
    public static List<StripsAction> GetAllActions()
    {
        List<StripsAction> actions = new List<StripsAction>();
        string[] blocks = { "A", "B", "C", "D", "E", "F" };
        string[] table = { "Table" };

        foreach (string b in blocks)
        {
            foreach (string x in blocks.Concat(table))
            {
                foreach (string y in blocks.Concat(table))
                {
                    if (b != x && b != y && x != y)
                    {
                        string actionName = $"Mover({b},{x},{y})";
                        
                        var preconds = new List<Predicate>();
                        preconds.Add(new Predicate("Encima", b, x));
                        preconds.Add(new Predicate("Libre", b));
                        if (y != "Table")
                        {
                            preconds.Add(new Predicate("Libre", y));
                        }
                        
                        var addEffects = new List<Predicate>();
                        addEffects.Add(new Predicate("Encima", b, y));
                        if (x != "Table")
                        {
                            addEffects.Add(new Predicate("Libre", x));
                        }
                        
                        var delEffects = new List<Predicate>();
                        delEffects.Add(new Predicate("Encima", b, x));
                        if (y != "Table")
                        {
                            delEffects.Add(new Predicate("Libre", y));
                        }
                        
                        actions.Add(new StripsAction(actionName, preconds, addEffects, delEffects));
                    }
                }
            }
        }
        return actions;
    }
}
