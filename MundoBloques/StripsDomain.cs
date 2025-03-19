using System;
using System.Collections.Generic;
using System.Linq;

public static class StripsDomain
{
    
    public static List<StripsAction> GetAllActions()
    {
        List<StripsAction> actions = new List<StripsAction>();
        // Se usan los bloques definidos en el escenario
        string[] blocks = { "A", "B", "C", "D", "E", "F" };
        // Las tres posiciones fijas de la mesa
        string[] tablePositions = { "T1", "T2", "T3" };

        // 1. Generar acciones Mover(b, x, y) para mover b desde x a otro bloque y.
        //    x se toma de (blocks ∪ tablePositions) y y se toma de blocks.
        foreach (string b in blocks)
        {
            foreach (string x in blocks.Concat(tablePositions))
            {
                foreach (string y in blocks)
                {
                    if (b != x && b != y && x != y)
                    {
                        string actionName = $"Mover({b},{x},{y})";
                        var preconds = new List<Predicate>()
                        {
                            new Predicate("Encima", b, x),
                            new Predicate("Libre", b),
                            new Predicate("Libre", y)
                        };
                        var addEffects = new List<Predicate>()
                        {
                            new Predicate("Encima", b, y),
                            new Predicate("Libre", x)
                        };
                        var delEffects = new List<Predicate>()
                        {
                            new Predicate("Encima", b, x),
                            new Predicate("Libre", y)
                        };
                        actions.Add(new StripsAction(actionName, preconds, addEffects, delEffects));
                    }
                }
            }
        }

        // 2. Generar acciones Mover(b, x, t) para mover b desde x a una posición de la mesa.
        foreach (string b in blocks)
        {
            foreach (string x in blocks.Concat(tablePositions))
            {
                if (b != x)
                {
                    foreach (string t in tablePositions)
                    {
                        if (x != t) // evitar mover b a la misma posición que ya ocupa
                        {
                            string actionName = $"Mover({b},{x},{t})";
                            var preconds = new List<Predicate>()
                            {
                                new Predicate("Encima", b, x),
                                new Predicate("Libre", b),
                                new Predicate("Libre", t)
                            };
                            var addEffects = new List<Predicate>()
                            {
                                new Predicate("Encima", b, t),
                                new Predicate("Libre", x)
                            };
                            var delEffects = new List<Predicate>()
                            {
                                new Predicate("Encima", b, x),
                                new Predicate("Libre", t)
                            };
                            actions.Add(new StripsAction(actionName, preconds, addEffects, delEffects));
                        }
                    }
                }
            }
        }
        return actions;
    }
}
