using System.Collections.Generic;

namespace AvaloniaApp.Models;

public class PersonalDatum
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string? Phone { get; set; }

    public string Email { get; set; } = null!;

    public string DateBirthday { get; set; } = null!;

    public string PassportSeries { get; set; } = null!;

    public string PassportNumber { get; set; } = null!;

    public byte[]? Photo { get; set; }

    public virtual ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
}
