using KalininaMargaritaKt_31_23.Database;
using KalininaMargaritaKt_31_23.Dto;
using KalininaMargaritaKt_31_23.Models;
using Microsoft.EntityFrameworkCore;
using NLog.Filters;
using System.Linq.Expressions;

namespace KalininaMargaritaKt_31_23.Interfaces
{
    public interface IDisciplineService
    {
        Task<ServiceResult<List<string>>> GetLastNamesStudentsWith2GradesByDisciplineNameAsync(DisciplineHas2GradeStudentFilter filter, CancellationToken ct = default);
        Task<ServiceResult<List<Discipline>>> GetDisciplinesAsync(CancellationToken ct = default);
        Task<ServiceResult<DisciplineDto>> AddDisciplineAsync(DisciplineRequest request, CancellationToken ct = default);
        Task<ServiceResult<DisciplineDto>> UpdateDisciplineAsync(int disciplineId, DisciplineRequest request, CancellationToken ct = default);
        Task<ServiceResult<bool>> DeleteDisciplineAsync(int disciplineId, CancellationToken ct = default);
    }

    public class DisciplineService : IDisciplineService
    {
        private static readonly Expression<Func<Discipline, DisciplineDto>> ToDto = d =>
            new DisciplineDto(d.DisciplineId, d.Name, d.IsDeleted);

        private readonly StudentDbContext _db;

        public DisciplineService(StudentDbContext db)
        {
            _db = db;
        }

        public async Task<ServiceResult<List<string>>> GetLastNamesStudentsWith2GradesByDisciplineNameAsync(DisciplineHas2GradeStudentFilter filter, CancellationToken ct = default)
        {
            if (!await _db.Disciplines.AnyAsync(d => d.Name == filter.DisciplineName, ct))
                return ServiceResult<List<string>>.NotFound("Дисциплина с таким именем не найдена");

            var lastNames = await _db.Students.AsNoTracking()
                .Where(s => s.Grades.Any(g => g.Value == 2 && g.Discipline.Name == filter.DisciplineName))
                .Where(s => s.IsDeleted == false)
                .Select(s => s.LastName)
                .ToListAsync(ct);

            return ServiceResult<List<string>>.Ok(lastNames);
        }

        public async Task<ServiceResult<List<Discipline>>> GetDisciplinesAsync(CancellationToken ct = default)
        {
            var query = await _db.Disciplines.AsNoTracking()
                .Where(q => q.IsDeleted == false)
                .ToListAsync(ct);

            return ServiceResult<List<Discipline>>.Ok(query);
        }

        public async Task<ServiceResult<DisciplineDto>> AddDisciplineAsync(DisciplineRequest request, CancellationToken ct = default)
        {
            var error = await ValidateAsync(request, 0, ct);
            if (error != null)
                return ServiceResult<DisciplineDto>.Invalid(error);

            var discipline = new Discipline
            {
                Name = request.Name!.Trim()
            };

            _db.Disciplines.Add(discipline);
            await _db.SaveChangesAsync(ct);

            return ServiceResult<DisciplineDto>.Ok(await GetDtoAsync(discipline.DisciplineId, ct));
        }

        public async Task<ServiceResult<DisciplineDto>> UpdateDisciplineAsync(int disciplineId, DisciplineRequest request, CancellationToken ct = default)
        {
            var discipline = await _db.Disciplines.FirstOrDefaultAsync(d => d.DisciplineId == disciplineId && !d.IsDeleted, ct);
            if (discipline == null)
                return ServiceResult<DisciplineDto>.NotFound("Дисциплина не найдена");

            var error = await ValidateAsync(request, disciplineId, ct);
            if (error != null)
                return ServiceResult<DisciplineDto>.Invalid(error);

            discipline.Name = request.Name!.Trim();
            await _db.SaveChangesAsync(ct);

            return ServiceResult<DisciplineDto>.Ok(await GetDtoAsync(disciplineId, ct));
        }

        public async Task<ServiceResult<bool>> DeleteDisciplineAsync(int disciplineId, CancellationToken ct = default)
        {
            var discipline = await _db.Disciplines.FirstOrDefaultAsync(d => d.DisciplineId == disciplineId && !d.IsDeleted, ct);
            if (discipline == null)
                return ServiceResult<bool>.NotFound("Дисциплина не найдена");

            discipline.IsDeleted = true;
            await _db.SaveChangesAsync(ct);

            return ServiceResult<bool>.Ok(true);
        }

        private Task<DisciplineDto> GetDtoAsync(int disciplineId, CancellationToken ct)
        {
            return _db.Disciplines.AsNoTracking().Where(d => d.DisciplineId == disciplineId).Select(ToDto).FirstAsync(ct);
        }

        private async Task<string?> ValidateAsync(DisciplineRequest request, int currentDisciplineId, CancellationToken ct)
        {
            var name = request.Name?.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return "Название дисциплины обязательно";
            if (name.Length > 150)
                return "Название дисциплины не должно быть длиннее 150 символов";
            if (await _db.Disciplines.AnyAsync(d => d.Name == name && !d.IsDeleted && d.DisciplineId != currentDisciplineId, ct))
                return "Дисциплина с таким названием уже существует";

            return null;
        }
    }
}