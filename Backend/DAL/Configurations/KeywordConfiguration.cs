namespace DAL.Configurations
{
    public class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
    {
        public void Configure(EntityTypeBuilder<Keyword> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(k => k.Word)
                .IsRequired()
                .HasMaxLength(50);

            // Relationships
            builder.HasMany(k => k.Listings)
                .WithMany(l => l.Keywords)
                .UsingEntity(j => j.ToTable("ListingKeywords"));
        }
    }
}













