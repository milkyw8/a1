namespace AvaloniaApp.Models;

public class ListUser
{
    public int UserId { get; set; }

    public long? TimeBanUser { get; set; }

    public string? Reason { get; set; }

    public virtual User User { get; set; } = null!;
}
