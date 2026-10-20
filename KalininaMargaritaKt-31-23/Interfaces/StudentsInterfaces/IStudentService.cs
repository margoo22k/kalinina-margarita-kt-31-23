using KalininaMargaritaKt_31_23.Database;
using KalininaMargaritaKt_31_23.Filters.StudentFilters;
using KalininaMargaritaKt_31_23.Models;
using Microsoft.EntityFrameworkCore;
using NLog.Filters;

namespace KalininaMargaritaKt_31_23.Interfaces.StudentsInterfaces
{
    public interface IStudentService
    {
        public Task<Student[]> GetStudentsByGroupAsync(StudentGroupFilter filter, CancellationToken cancellationToken);
    }

    public class StudentService : IStudentService
    {
        private readonly StudentDbContext _dbContext;

        public StudentService(StudentDbContext dbContext)
        { 
            _dbContext = dbContext;
        }

        public Task<Student[]> GetStudentsByGroupAsync(StudentGroupFilter filter, CancellationToken cancellationToken = default)
        {
            var students = _dbContext.Set<Student>().Where(w => w.Group.Name == filter.GroupName).ToArrayAsync(cancellationToken);
            return students;
        }
    }
}
