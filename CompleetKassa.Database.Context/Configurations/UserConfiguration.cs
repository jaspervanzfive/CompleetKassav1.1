using CompleetKassa.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompleetKassa.Database.Context.Configurations
{
	public class UserConfiguration : EntityMappingConfiguration<User>
	{
		public override void Map(EntityTypeBuilder<User> builder)
		{
			builder.Property(t => t.ID).ValueGeneratedOnAdd();
			builder.HasKey(t => t.ID);
			builder.HasIndex(t => t.ID);

			// Set concurrency token for entity
			builder.Property(t => t.Timestamp)
				.ValueGeneratedOnAddOrUpdate()
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.IsRowVersion();

			// One To One Relation
			builder.HasOne(t => t.UserCredential)
					.WithOne(t => t.User)
					.HasForeignKey<UserCredential>(t => t.UserID)
					.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
