using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Entities;
using WebApi.Infrastructure.Identity;

namespace WebApi.Infrastructure.Data.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {

        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}