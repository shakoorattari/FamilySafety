using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FamilySafety.Domain.Entities;

namespace FamilySafety.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.ExternalId)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(u => u.ProfileImageUrl)
            .HasMaxLength(500);

        builder.Property(u => u.DeviceToken)
            .HasMaxLength(500);

        builder.Property(u => u.TimeZone)
            .HasMaxLength(100);

        builder.HasIndex(u => u.ExternalId).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
    }
}

public class FamilyGroupConfiguration : IEntityTypeConfiguration<FamilyGroup>
{
    public void Configure(EntityTypeBuilder<FamilyGroup> builder)
    {
        builder.ToTable("FamilyGroups");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.Description)
            .HasMaxLength(500);

        builder.Property(f => f.ImageUrl)
            .HasMaxLength(500);

        builder.Property(f => f.InviteCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(f => f.InviteCode).IsUnique();
    }
}

public class FamilyMemberConfiguration : IEntityTypeConfiguration<FamilyMember>
{
    public void Configure(EntityTypeBuilder<FamilyMember> builder)
    {
        builder.ToTable("FamilyMembers");

        builder.HasKey(m => m.Id);

        builder.HasOne(m => m.User)
            .WithMany(u => u.FamilyMemberships)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.FamilyGroup)
            .WithMany(f => f.Members)
            .HasForeignKey(m => m.FamilyGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => new { m.UserId, m.FamilyGroupId }).IsUnique();
    }
}

public class GeofenceConfiguration : IEntityTypeConfiguration<Geofence>
{
    public void Configure(EntityTypeBuilder<Geofence> builder)
    {
        builder.ToTable("Geofences");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.Description)
            .HasMaxLength(500);

        builder.HasOne(g => g.FamilyGroup)
            .WithMany(f => f.Geofences)
            .HasForeignKey(g => g.FamilyGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class LocationHistoryConfiguration : IEntityTypeConfiguration<LocationHistory>
{
    public void Configure(EntityTypeBuilder<LocationHistory> builder)
    {
        builder.ToTable("LocationHistories");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Address)
            .HasMaxLength(500);

        builder.HasOne(l => l.User)
            .WithMany(u => u.LocationHistories)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(l => new { l.UserId, l.RecordedAt });
    }
}

public class SafetyAlertConfiguration : IEntityTypeConfiguration<SafetyAlert>
{
    public void Configure(EntityTypeBuilder<SafetyAlert> builder)
    {
        builder.ToTable("SafetyAlerts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Message)
            .HasMaxLength(1000);

        builder.Property(a => a.Address)
            .HasMaxLength(500);

        builder.Property(a => a.ResolutionNotes)
            .HasMaxLength(1000);

        builder.HasOne(a => a.User)
            .WithMany(u => u.SafetyAlerts)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.FamilyGroup)
            .WithMany()
            .HasForeignKey(a => a.FamilyGroupId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(a => new { a.UserId, a.TriggeredAt });
        builder.HasIndex(a => a.Status);
    }
}
