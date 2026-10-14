using KalininaMargaritaKt_31_23.Database.Helpers;
using KalininaMargaritaKt_31_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KalininaMargaritaKt_31_23.Database.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {

        private const string TableName = "cd_student";
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(p => p.StudentId)
                   .HasName($"pk_{TableName}_student_id");

            builder.Property(p => p.StudentId)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("c_student_id")
                   .HasComment("Идентификатор записи студента");

            builder.Property(p => p.FirstName)
                   .IsRequired()
                   .HasColumnName("c_student_firstname")
                   .HasColumnType(ColumnType.String).HasMaxLength(100)
                   .HasComment("Имя студента");

            builder.Property(p => p.LastName)
                   .IsRequired()
                   .HasColumnName("c_student_lastname")
                   .HasColumnType(ColumnType.String).HasMaxLength(100)
                   .HasComment("Фамилия студента");

            builder.Property(p => p.IsDeleted)
                   .IsRequired()
                   .HasColumnName("c_student_is_deleted")
                   .HasColumnType(ColumnType.Bool)
                   .HasComment("Признак удаления");

            // Связь 1:N с Group
            builder.HasOne(p => p.Group)
                   .WithMany(p => p.Students)
                   .HasForeignKey(p => p.GroupId)
                   .HasConstraintName("fk_f_group_id")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.GroupId, $"idx_{TableName}_fk_f_group_id");

            // Добавление явной автоподгрузки связанной сущности
            builder.Navigation(p => p.Group).AutoInclude();

        }

    }
}
