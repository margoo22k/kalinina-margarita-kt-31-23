using KalininaMargaritaKt_31_23.Database.Helpers;
using KalininaMargaritaKt_31_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KalininaMargaritaKt_31_23.Database.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {

        private const string TableName = "cd_group";

        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(p => p.GroupId)
                   .HasName($"pk_{TableName}_group_id");

            builder.Property(p => p.GroupId)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("c_group_id")
                   .HasComment("Идентификатор группы");

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasColumnName("c_group_name")
                   .HasColumnType(ColumnType.String).HasMaxLength(100)
                   .HasComment("Название группы");

            builder.Property(p => p.Course)
                   .IsRequired()
                   .HasColumnName("c_group_course")
                   .HasColumnType(ColumnType.Int)
                   .HasComment("Номер курса");

            builder.Property(p => p.IsDeleted)
                   .IsRequired()
                   .HasColumnName("c_group_is_deleted")
                   .HasColumnType(ColumnType.Bool)
                   .HasComment("Признак удаления");

            // Связь 1:N со Specialty
            builder.HasOne(p => p.Specialty)
                   .WithMany(p => p.Groups)
                   .HasForeignKey(p => p.SpecialtyId)
                   .HasConstraintName("fk_f_specialty_id")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.SpecialtyId, $"idx_{TableName}_fk_f_specialty_id");

            builder.Navigation(p => p.Specialty).AutoInclude();
        }

    }

}
