using Microsoft.EntityFrameworkCore;

namespace TheVilleSkill.Data
{
    public partial class LouisvilleDemographicsContext : DbContext
    {
        public LouisvilleDemographicsContext() { }
        
        public LouisvilleDemographicsContext(DbContextOptions<LouisvilleDemographicsContext> options) : base(options) { }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<AddressAttribute> AddressAttributes { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<TagCommonAbbreviation> TagCommonAbbreviations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.Direction)
                    .HasColumnName("direction")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasColumnName("street")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .HasColumnName("zip")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(e => e.Tag1)
                    .WithMany(e => e.Addresses)
                    .HasForeignKey(e => e.Tag)
                    .HasConstraintName("FK_Address_Tag");
            });

            modelBuilder.Entity<AddressAttribute>(entity =>
            {
                entity.ToTable("AddressAttribute");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AttributeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AttributeValue)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(e => e.Address)
                    .WithMany(e => e.Attributes)
                    .HasForeignKey(e => e.AddressId)
                    .HasConstraintName("FK_AddressAttribute_Attribute");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");

                entity.HasKey(e => e.USPSStandardAbbreviation);

                entity.Property(e => e.USPSStandardAbbreviation)
                    .HasColumnName("USPSStandardAbbreviation")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TagCommonAbbreviation>(entity =>
            {
                entity.ToTable("TagCommonAbbreviation");

                entity.HasKey(e => new { e.USPSStandardAbbreviation, e.CommonAbbreviation });

                entity.Property(e => e.USPSStandardAbbreviation)
                    .HasColumnName("USPSStandardAbbreviation")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CommonAbbreviation)
                    .HasColumnName("CommonAbbreviation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(e => e.Tag)
                    .WithMany(e => e.CommonAbbreviations)
                    .HasForeignKey(e => e.USPSStandardAbbreviation)
                    .HasConstraintName("FK_TagCommonAbbreviation_Tag");
            });
        }
    }
}
