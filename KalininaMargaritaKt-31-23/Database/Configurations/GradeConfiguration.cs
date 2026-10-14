using KalininaMargaritaKt_31_23.Database.Helpers;
using KalininaMargaritaKt_31_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KalininaMargaritaKt_31_23.Database.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {

        private const string TableName = "cd_grade";

        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(p => p.GradeId)
                   .HasName($"pk_{TableName}_grade_id");

            builder.Property(p => p.GradeId)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("c_grade_id")
                   .HasComment("Идентификатор оценки");

            builder.Property(p => p.Value)
                   .IsRequired()
                   .HasColumnName("c_grade_value")
                   .HasColumnType(ColumnType.Int)
                   .HasComment("Значение оценки");

            // Связь 1:N со Student
            builder.HasOne(p => p.Student)
                   .WithMany(p => p.Grades)
                   .HasForeignKey(p => p.StudentId)
                   .HasConstraintName("fk_f_student_id")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.StudentId, $"idx_{TableName}_fk_f_student_id");

            // Связь 1:N с Discipline
            builder.HasOne(p => p.Discipline)
                   .WithMany(p => p.Grades)
                   .HasForeignKey(p => p.DisciplineId)
                   .HasConstraintName("fk_f_discipline_id")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.DisciplineId, $"idx_{TableName}_fk_f_discipline_id");
        }

    }

}
