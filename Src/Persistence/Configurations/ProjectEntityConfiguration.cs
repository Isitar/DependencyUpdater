namespace Isitar.DependencyUpdater.Persistence.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasOne(p => p.Platform)
                .WithMany()
                .HasForeignKey(p => p.PlatformId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}