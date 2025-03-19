public static class Scenario
{
    // Retorna el estado inicial del escenario.
    public static WorldState GetInitialState()
    {
        Dictionary<string, string> initialPositions = new Dictionary<string, string>();
        // Definir el estado inicial: por ejemplo, A sobre B, B sobre C y C en la posición T1.
        initialPositions.Add("A", "T1");
        initialPositions.Add("B", "A");
        initialPositions.Add("C", "B");
        initialPositions.Add("D", "T3");
        initialPositions.Add("E", "T2");
        initialPositions.Add("F", "E");
        return new WorldState(initialPositions);
    }

    // Retorna el estado objetivo del escenario.
    public static WorldState GetGoalState()
    {
        Dictionary<string, string> goalPositions = new Dictionary<string, string>();
        // Definir el estado objetivo: por ejemplo, B sobre A, A sobre C y C en la posición T1.
        goalPositions.Add("A", "T3");
        goalPositions.Add("B", "T2");
        goalPositions.Add("C", "T1");
        goalPositions.Add("D", "A");
        goalPositions.Add("E", "C");
        goalPositions.Add("F", "B");
        return new WorldState(goalPositions);
    }
}