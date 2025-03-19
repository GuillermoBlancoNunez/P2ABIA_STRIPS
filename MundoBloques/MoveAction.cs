// Representa una acción de mover un bloque desde un soporte a otro.
public class MoveAction
{
    public string Block { get; set; }
    public string From { get; set; }
    public string To { get; set; }

    public MoveAction(string block, string from, string to)
    {
        this.Block = block;
        this.From = from;
        this.To = to;
    }

    // Método auxiliar: determina si una cadena representa una posición de la mesa.
    public static bool IsTablePosition(string pos)
    {
        return (pos == "T1" || pos == "T2" || pos == "T3");
    }

    // Comprueba si la acción es aplicable en el estado dado.
    public bool IsApplicable(WorldState state)
    {
        if (!state.Positions.ContainsKey(this.Block) || state.Positions[this.Block] != this.From)
        {
            return false;
        }
        if (!state.IsClear(this.Block))
        {
            return false;
        }
        // Si el destino es una posición de la mesa, se verifica que esté libre.
        if (IsTablePosition(this.To))
        {
            foreach (string supp in state.Positions.Values)
            {
                if (supp == this.To)
                {
                    return false;
                }
            }
        }
        else
        {
            if (!state.IsClear(this.To))
            {
                return false;
            }
        }
        return true;
    }

    // Aplica la acción sobre el estado y retorna el nuevo estado.
    public WorldState Apply(WorldState state)
    {
        if (!this.IsApplicable(state))
        {
            throw new Exception("Acción no aplicable: Mover " + this.Block + " de " + this.From + " a " + this.To);
        }
        WorldState newState = state.Clone();
        newState.Positions[this.Block] = this.To;
        return newState;
    }

    public override string ToString()
    {
        string fromStr = (IsTablePosition(this.From)) ? "posición " + this.From : this.From;
        string toStr = (IsTablePosition(this.To)) ? "posición " + this.To : this.To;
        return "Mover " + this.Block + " de " + fromStr + " a " + toStr;
    }
}