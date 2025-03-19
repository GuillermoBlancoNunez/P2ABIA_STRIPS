// Representa el estado del mundo de bloques.
public class WorldState
{
    public Dictionary<string, string> Positions { get; set; }

    public WorldState(Dictionary<string, string> positions)
    {
        this.Positions = new Dictionary<string, string>(positions);
    }

    // Un bloque está libre si ningún otro bloque se encuentra sobre él.
    public bool IsClear(string block)
    {
        return !this.Positions.Values.Contains(block);
    }

    // Retorna una copia del estado actual.
    public WorldState Clone()
    {
        return new WorldState(new Dictionary<string, string>(this.Positions));
    }

    // Representación única del estado, usada para la comparación.
    public string ToStringRepresentation()
    {
        return string.Join(";", this.Positions.OrderBy(k => k.Key)
            .Select(k => k.Key + ":" + k.Value));
    }

    public override bool Equals(object obj)
    {
        if (obj is WorldState)
        {
            WorldState other = (WorldState)obj;
            if (this.Positions.Count != other.Positions.Count)
            {
                return false;
            }
            foreach (KeyValuePair<string, string> kvp in this.Positions)
            {
                if (!other.Positions.ContainsKey(kvp.Key) || other.Positions[kvp.Key] != kvp.Value)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (KeyValuePair<string, string> kvp in this.Positions.OrderBy(k => k.Key))
        {
            hash = hash * 23 + kvp.Key.GetHashCode();
            hash = hash * 23 + (kvp.Value != null ? kvp.Value.GetHashCode() : 0);
        }
        return hash;
    }
}