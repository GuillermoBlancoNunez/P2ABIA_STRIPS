using System;
using System.Collections.Generic;


public class Agent
{
    public HashSet<Predicate> CurrentState { get; set; }
    public HashSet<Predicate> GoalState { get; set; }
    public List<StripsAction> Plan { get; set; }
    public StripsPlanner Planner { get; set; }

    public Agent(HashSet<Predicate> initial, HashSet<Predicate> goal)
    {
        this.CurrentState = new HashSet<Predicate>(initial);
        this.GoalState = new HashSet<Predicate>(goal);
        this.Planner = new StripsPlanner();
        this.Plan = this.Planner.GetPlan(initial, goal);
    }

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
