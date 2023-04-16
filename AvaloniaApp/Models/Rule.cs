using System.Collections.Generic;

namespace AvaloniaApp.Models;

public class Rule
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<PositionRule> PositionRules { get; } = new List<PositionRule>();
}
