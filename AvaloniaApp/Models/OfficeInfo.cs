using System.Collections.Generic;

namespace AvaloniaApp.Models;

public class OfficeInfo
{
    public int Id { get; set; }

    public string? Subdivision { get; set; }

    public string? Department { get; set; }

    public int? PositionRuleId { get; set; }

    public virtual PositionRule? PositionRule { get; set; }

    public virtual ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
}
