using ApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTracker.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    public DbSet<StatusHistory> StatusHistories => Set<StatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(application => application.Id);
            entity.Property(application => application.Company).HasMaxLength(200).IsRequired();
            entity.Property(application => application.Position).HasMaxLength(200).IsRequired();
            entity.Property(application => application.Location).HasMaxLength(200);
            entity.Property(application => application.Source).HasMaxLength(200);
            entity.Property(application => application.ExternalJobId).HasMaxLength(64);
            entity.Property(application => application.Url).HasMaxLength(2048).IsRequired();
            entity.Property(application => application.Notes).HasMaxLength(4000);
            entity.Property(application => application.SalaryFrom).HasPrecision(18, 2);
            entity.Property(application => application.SalaryTo).HasPrecision(18, 2);
            entity.HasIndex(application => application.ExternalJobId).IsUnique();
            entity.Ignore(application => application.NormalizedCompany);
            entity.Ignore(application => application.SalaryRange);
            entity.Ignore(application => application.CurrentStatus);
            entity.Ignore(application => application.Status);

            entity.HasMany(application => application.History)
                .WithOne(history => history.JobApplication)
                .HasForeignKey(history => history.JobApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StatusHistory>(entity =>
        {
            entity.HasKey(history => history.Id);
            entity.Property(history => history.Note).HasMaxLength(2000);
            entity.Property(history => history.Status).IsRequired();
        });
    }
}
