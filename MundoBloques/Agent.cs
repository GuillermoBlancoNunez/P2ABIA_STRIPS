/// <summary>
/// Represents an agent in the blocks world that computes and executes a STRIPS plan.
/// </summary>
public class Agent
{
    /// <summary>
    /// Gets or sets the current state of the agent.
    /// </summary>
    public HashSet<Predicate> CurrentState { get; set; }

    /// <summary>
    /// Gets or sets the goal state the agent aims to achieve.
    /// </summary>
    public HashSet<Predicate> GoalState { get; set; }

    /// <summary>
    /// Gets or sets the plan (list of actions) computed by the planner.
    /// </summary>
    public List<StripsAction> Plan { get; set; }

    /// <summary>
    /// Gets or sets the STRIPS planner used to compute the plan.
    /// </summary>
    public StripsPlanner Planner { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Agent"/> class using the specified initial and goal states.
    /// </summary>
    /// <param name="initial">The initial state as a set of predicates.</param>
    /// <param name="goal">The goal state as a set of predicates.</param>
    public Agent(HashSet<Predicate> initial, HashSet<Predicate> goal)
    {
        this.CurrentState = new HashSet<Predicate>(initial);
        this.GoalState = new HashSet<Predicate>(goal);
        this.Planner = new StripsPlanner();
        this.Plan = this.Planner.GetPlan(initial, goal);
    }

    /// <summary>
    /// Executes the computed plan step by step, updating the current state.
    /// </summary>
    /// <returns>An enumerable collection of actions executed.</returns>
    public IEnumerable<StripsAction> ExecutePlan()
    {
        foreach (var action in this.Plan)
        {
            if (action.IsApplicable(this.CurrentState))
            {
                this.CurrentState = action.Apply(this.CurrentState);
                yield return action;
            }
            else
            {
                throw new Exception("Acción no aplicable durante la ejecución: " + action.Name);
            }
        }
    }
}
