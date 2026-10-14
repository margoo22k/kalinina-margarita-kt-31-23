using KalininaMargaritaKt_31_23.Models;
using Microsoft.EntityFrameworkCore;

namespace KalininaMargaritaKt_31_23.Database 
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {
        }
            public DbSet<Specialty> Specialtys { get; set; }
            public DbSet<Group> Groups { get; set; }
            public DbSet<Student> Students { get; set; }
            public DbSet<Discipline> Disciplines { get; set; }
            public DbSet<Grade> Grades { get; set; }
        

    }
}