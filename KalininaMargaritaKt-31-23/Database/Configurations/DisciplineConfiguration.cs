using KalininaMargaritaKt_31_23.Database.Helpers;
using KalininaMargaritaKt_31_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KalininaMargaritaKt_31_23.Database.Configurations
{
    public class DisciplineConfiguration : IEntityTypeConfiguration<Discipline>
    {

        private const string TableName = "cd_discipline";

        public void Configure(EntityTypeBuilder<Discipline> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(p => p.DisciplineId)
                   .HasName($"pk_{TableName}_discipline_id");

            builder.Property(p => p.DisciplineId)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("c_discipline_id")
                   .HasComment("Идентификатор дисциплины");

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasColumnName("c_discipline_name")
                   .HasColumnType(ColumnType.String).HasMaxLength(100)
                   .HasComment("Название дисциплины");

            builder.Property(p => p.IsDeleted)
                   .IsRequired()
                   .HasColumnName("c_discipline_is_deleted")
                   .HasColumnType(ColumnType.Bool)
                   .HasComment("Признак удаления");
        }

    }

}
