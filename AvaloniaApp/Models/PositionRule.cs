using System.Collections.Generic;

namespace AvaloniaApp.Models;

public class PositionRule
{
    public int Id { get; set; }

    public int RuleId { get; set; }

    public int PositionId { get; set; }

    public virtual ICollection<OfficeInfo> OfficeInfos { get; } = new List<OfficeInfo>();

    public virtual Position Position { get; set; } = null!;

    public virtual Rule Rule { get; set; } = null!;
}
