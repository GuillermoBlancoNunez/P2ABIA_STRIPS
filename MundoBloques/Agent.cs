// Agente que integra el planificador y ejecuta el plan para un escenario dado.
public class Agent
{
    public WorldState CurrentState { get; set; }
    public WorldState GoalState { get; set; }
    public List<MoveAction> Plan { get; set; }
    public Planner Planner { get; set; }

    public Agent(WorldState initial, WorldState goal)
    {
        this.CurrentState = initial;
        this.GoalState = goal;
        this.Planner = new Planner();
        this.Plan = this.Planner.GetPlan(initial, goal);
    }

    public IEnumerable<MoveAction> ExecutePlan()
    {
        foreach (MoveAction action in this.Plan)
        {
            if (action.IsApplicable(this.CurrentState))
            {
                this.CurrentState = action.Apply(this.CurrentState);
                yield return action;
            }
            else
            {
                throw new Exception("Fallo en la ejecución del plan: acción no aplicable.");
            }
        }
    }
}
