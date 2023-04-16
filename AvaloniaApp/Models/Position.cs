using System.Collections.Generic;

namespace AvaloniaApp.Models;

public class Position
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<PositionRule> PositionRules { get; } = new List<PositionRule>();
}
