using System.Collections.Generic;

namespace AvaloniaApp.Models;

public class RequestStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
}
