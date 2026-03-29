using Infrastructure.Identity;
using Infrastructure.Persistence.Entities.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfCore.Configurations.Members;

internal class MemberEntityConfiguration : IEntityTypeConfiguration<MemberEntity>
{
    public void Configure(EntityTypeBuilder<MemberEntity> builder)
    {
        builder.ToTable("Members");

        builder.Property(x => x.Id)
            .IsRequired();



        builder.Property(x => x.FirstName)
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .HasMaxLength(100);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(50);

        builder.Property(x => x.ProfileImageUri)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.ModifiedAt);


        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder
            .HasOne<ApplicationUser>()
            .WithOne(x => x.Member)
            .HasForeignKey<MemberEntity>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
