using System.Linq.Expressions;
using KalininaMargaritaKt_31_23.Database;
using KalininaMargaritaKt_31_23.Dto;
using KalininaMargaritaKt_31_23.Models;
using Microsoft.EntityFrameworkCore;

namespace KalininaMargaritaKt_31_23.Interfaces
{
    public interface IStudentService
    {
        Task<ServiceResult<List<Student>>> GetStudentsAsync(CancellationToken ct = default);
        Task<ServiceResult<StudentDto>> AddStudentAsync(StudentRequest request, CancellationToken ct = default);
        Task<ServiceResult<StudentDto>> UpdateStudentAsync(int studentId, StudentRequest request, CancellationToken ct = default);
        Task<ServiceResult<bool>> DeleteStudentAsync(int studentId, CancellationToken ct = default);
    }

    public class StudentService : IStudentService
    {
        private static readonly Expression<Func<Student, StudentDto>> ToDto = s =>
            new StudentDto(s.StudentId, s.FirstName, s.LastName, s.GroupId, s.Group!.Name, s.IsDeleted);

        private readonly StudentDbContext _db;
        public StudentService(StudentDbContext db) => _db = db;

        public async Task<ServiceResult<List<Student>>> GetStudentsAsync(CancellationToken ct = default)
        {
            var query = await _db.Students.AsNoTracking()
                .Where(q => q.IsDeleted == false)
                .ToListAsync(ct);

            return ServiceResult<List<Student>>.Ok(query);
        }

        public async Task<ServiceResult<StudentDto>> AddStudentAsync(StudentRequest request, CancellationToken ct = default)
        {
            var error = await ValidateAsync(request, ct);
            if (error != null) return ServiceResult<StudentDto>.Invalid(error);

            var student = new Student { FirstName = request.FirstName!.Trim(), LastName = request.LastName!.Trim(), GroupId = request.GroupId };
            _db.Students.Add(student);
            await _db.SaveChangesAsync(ct);
            return ServiceResult<StudentDto>.Ok(await GetDtoAsync(student.StudentId, ct));
        }

        public async Task<ServiceResult<StudentDto>> UpdateStudentAsync(int studentId, StudentRequest request, CancellationToken ct = default)
        {
            var student = await _db.Students.FirstOrDefaultAsync(s => s.StudentId == studentId && !s.IsDeleted, ct);
            if (student == null) return ServiceResult<StudentDto>.NotFound("Студент не найден");

            var error = await ValidateAsync(request, ct);
            if (error != null) return ServiceResult<StudentDto>.Invalid(error);

            student.FirstName = request.FirstName!.Trim(); student.LastName = request.LastName!.Trim(); student.GroupId = request.GroupId;
            await _db.SaveChangesAsync(ct);
            return ServiceResult<StudentDto>.Ok(await GetDtoAsync(studentId, ct));
        }

        public async Task<ServiceResult<bool>> DeleteStudentAsync(int studentId, CancellationToken ct = default)
        {
            var student = await _db.Students.FirstOrDefaultAsync(s => s.StudentId == studentId && !s.IsDeleted, ct);
            if (student == null) return ServiceResult<bool>.NotFound("Студент не найден");

            student.IsDeleted = true;
            await _db.SaveChangesAsync(ct);
            return ServiceResult<bool>.Ok(true);
        }

        private Task<StudentDto> GetDtoAsync(int studentId, CancellationToken ct) =>
            _db.Students.AsNoTracking().Where(s => s.StudentId == studentId).Select(ToDto).FirstAsync(ct);

        private async Task<string?> ValidateAsync(StudentRequest request, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName?.Trim())) return "Имя обязательно";
            if (string.IsNullOrWhiteSpace(request.LastName?.Trim())) return "Фамилия обязательна";
            if (!await _db.Groups.AnyAsync(g => g.GroupId == request.GroupId && !g.IsDeleted, ct)) return "Группа не найдена";
            return null;
        }
    }
}