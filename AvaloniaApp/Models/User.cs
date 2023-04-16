using System.Collections.Generic;

namespace AvaloniaApp.Models;

public class User
{
    public int Id { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public int RoleId { get; set; }

    public bool IsBlocked { get; set; }

    public bool AccountConfirmed { get; set; }

    public string? Gender { get; set; }

    public string? SecretWord { get; set; }

    public long? DateTimeWindowBlock { get; set; }

    public bool? BreakeRuless { get; set; }

    public virtual ListUser? ListUser { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
}
