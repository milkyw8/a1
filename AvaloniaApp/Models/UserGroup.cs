namespace AvaloniaApp.Models;

public class UserGroup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int PersonId { get; set; }

    public int? UserId { get; set; }

    public int? EmployeeId { get; set; }

    public string TypeVisit { get; set; } = null!;

    public string Organization { get; set; } = null!;

    public string? Comment { get; set; }

    public int TargetVisitId { get; set; }

    public int RequestStatusId { get; set; }

    public long? DateRequest { get; set; }

    public long ValidFrom { get; set; }

    public long ValidUntil { get; set; }

    public long? VisitTime { get; set; }

    public long? LeavingTime { get; set; }

    public virtual OfficeInfo? Employee { get; set; }

    public virtual PersonalDatum Person { get; set; } = null!;

    public virtual RequestStatus RequestStatus { get; set; } = null!;

    public virtual TargetVisit TargetVisit { get; set; } = null!;

    public virtual User? User { get; set; }
}
