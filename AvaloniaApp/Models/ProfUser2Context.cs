using Microsoft.EntityFrameworkCore;

namespace AvaloniaApp.Models;

public partial class ProfUser2Context : DbContext
{
    public ProfUser2Context()
    {
    }

    public ProfUser2Context(DbContextOptions<ProfUser2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<ListUser> ListUsers { get; set; }

    public virtual DbSet<OfficeInfo> OfficeInfos { get; set; }

    public virtual DbSet<PersonalDatum> PersonalData { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<PositionRule> PositionRules { get; set; }

    public virtual DbSet<RequestStatus> RequestStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<TargetVisit> TargetVisits { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=81.177.6.104;user=prof_user1;password=WsrWsrWsr2021$;database=prof_user2",
            ServerVersion.Parse("10.5.19-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<ListUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("ListUser");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.Reason).HasMaxLength(5000);
            entity.Property(e => e.TimeBanUser).HasColumnType("bigint(20)");

            entity.HasOne(d => d.User).WithOne(p => p.ListUser)
                .HasForeignKey<ListUser>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ListUser_FK");
        });

        modelBuilder.Entity<OfficeInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("OfficeInfo");

            entity.HasIndex(e => e.PositionRuleId, "Employee_FK");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.PositionRuleId).HasColumnType("int(11)");
            entity.Property(e => e.Subdivision).HasMaxLength(100);

            entity.HasOne(d => d.PositionRule).WithMany(p => p.OfficeInfos)
                .HasForeignKey(d => d.PositionRuleId)
                .HasConstraintName("Employee_FK");
        });

        modelBuilder.Entity<PersonalDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.DateBirthday).HasMaxLength(10);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Firstname).HasMaxLength(100);
            entity.Property(e => e.Lastname).HasMaxLength(100);
            entity.Property(e => e.PassportNumber).HasMaxLength(6);
            entity.Property(e => e.PassportSeries).HasMaxLength(4);
            entity.Property(e => e.Patronymic).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Photo).HasColumnType("mediumblob");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Position");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PositionRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("PositionRule");

            entity.HasIndex(e => e.PositionId, "PositionRule_FK");

            entity.HasIndex(e => e.RuleId, "Structure_FK");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.PositionId).HasColumnType("int(11)");
            entity.Property(e => e.RuleId).HasColumnType("int(11)");

            entity.HasOne(d => d.Position).WithMany(p => p.PositionRules)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PositionRule_FK");

            entity.HasOne(d => d.Rule).WithMany(p => p.PositionRules)
                .HasForeignKey(d => d.RuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Structure_FK");
        });

        modelBuilder.Entity<RequestStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("RequestStatus");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Rule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Rule");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TargetVisit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("TargetVisit");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User");

            entity.HasIndex(e => e.RoleId, "User_FK");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.DateTimeWindowBlock).HasColumnType("bigint(20)");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.RoleId).HasColumnType("int(11)");
            entity.Property(e => e.SecretWord).HasMaxLength(10);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_FK");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("UserGroup");

            entity.HasIndex(e => e.PersonId, "UserGroup_FK");

            entity.HasIndex(e => e.EmployeeId, "UserGroup_FK_1");

            entity.HasIndex(e => e.UserId, "UserGroup_FK_3");

            entity.HasIndex(e => e.TargetVisitId, "UserGroup_FK_4");

            entity.HasIndex(e => e.RequestStatusId, "UserGroup_FK_6");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Comment).HasMaxLength(5000);
            entity.Property(e => e.DateRequest).HasColumnType("bigint(20)");
            entity.Property(e => e.EmployeeId).HasColumnType("int(11)");
            entity.Property(e => e.LeavingTime).HasColumnType("bigint(20)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Organization).HasMaxLength(100);
            entity.Property(e => e.PersonId).HasColumnType("int(11)");
            entity.Property(e => e.RequestStatusId).HasColumnType("int(11)");
            entity.Property(e => e.TargetVisitId).HasColumnType("int(11)");
            entity.Property(e => e.TypeVisit).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnType("int(11)");
            entity.Property(e => e.ValidFrom).HasColumnType("bigint(20)");
            entity.Property(e => e.ValidUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.VisitTime).HasColumnType("bigint(20)");

            entity.HasOne(d => d.Employee).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("UserGroup_FK_1");

            entity.HasOne(d => d.Person).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserGroup_FK");

            entity.HasOne(d => d.RequestStatus).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.RequestStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserGroup_FK_6");

            entity.HasOne(d => d.TargetVisit).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.TargetVisitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserGroup_FK_4");

            entity.HasOne(d => d.User).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserGroup_FK_3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
