using System.Linq.Expressions;
using KalininaMargaritaKt_31_23.Database;
using KalininaMargaritaKt_31_23.Dto;
using KalininaMargaritaKt_31_23.Models;
using Microsoft.EntityFrameworkCore;

namespace KalininaMargaritaKt_31_23.Interfaces
{
    public interface IGroupService
    {
        Task<ServiceResult<List<Group>>> GetGroupsAsync(CancellationToken ct = default);
        Task<ServiceResult<GroupDto>> AddGroupAsync(GroupRequest request, CancellationToken ct = default);
        Task<ServiceResult<GroupDto>> UpdateGroupAsync(int groupId, GroupRequest request, CancellationToken ct = default);
        Task<ServiceResult<bool>> DeleteGroupAsync(int groupId, CancellationToken ct = default);
    }

    public class GroupService : IGroupService
    {
        private static readonly Expression<Func<Group, GroupDto>> ToDto = g =>
            new GroupDto(g.GroupId, g.Name, g.Course, g.SpecialtyId, g.Specialty!.Title, g.IsDeleted);

        private readonly StudentDbContext _db;
        public GroupService(StudentDbContext db) => _db = db;


        public async Task<ServiceResult<List<Group>>> GetGroupsAsync(CancellationToken ct = default)
        {
            var query = await _db.Groups.AsNoTracking()
                .Where(q => q.IsDeleted == false)
                .ToListAsync(ct);

            return ServiceResult<List<Group>>.Ok(query);
        }

        public async Task<List<GroupDto>> GetGroupsAsync(GroupFilter filter, CancellationToken ct = default)
        {
            var query = _db.Groups.AsNoTracking();
            if (filter.SpecialtyId.HasValue) query = query.Where(g => g.SpecialtyId == filter.SpecialtyId.Value);
            if (filter.Course.HasValue) query = query.Where(g => g.Course == filter.Course.Value);
            if (filter.IsDeleted.HasValue) query = query.Where(g => g.IsDeleted == filter.IsDeleted.Value);
            return await query.OrderBy(g => g.Name).Select(ToDto).ToListAsync(ct);
        }

        public async Task<ServiceResult<GroupDto>> AddGroupAsync(GroupRequest request, CancellationToken ct = default)
        {
            var error = await ValidateAsync(request, 0, ct);
            if (error != null) return ServiceResult<GroupDto>.Invalid(error);

            var group = new Group { Name = request.Name!.Trim(), Course = request.Course, SpecialtyId = request.SpecialtyId };
            _db.Groups.Add(group);
            await _db.SaveChangesAsync(ct);
            return ServiceResult<GroupDto>.Ok(await GetDtoAsync(group.GroupId, ct));
        }

        public async Task<ServiceResult<GroupDto>> UpdateGroupAsync(int groupId, GroupRequest request, CancellationToken ct = default)
        {
            var group = await _db.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId && !g.IsDeleted, ct);
            if (group == null) return ServiceResult<GroupDto>.NotFound("Группа не найдена");

            var error = await ValidateAsync(request, groupId, ct);
            if (error != null) return ServiceResult<GroupDto>.Invalid(error);

            group.Name = request.Name!.Trim(); group.Course = request.Course; group.SpecialtyId = request.SpecialtyId;
            await _db.SaveChangesAsync(ct);
            return ServiceResult<GroupDto>.Ok(await GetDtoAsync(groupId, ct));
        }

        public async Task<ServiceResult<bool>> DeleteGroupAsync(int groupId, CancellationToken ct = default)
        {
            // ВАЖНО: Загружаем студентов через Include, чтобы пометить их как удаленных вместе с группой
            var group = await _db.Groups.Include(g => g.Students).FirstOrDefaultAsync(g => g.GroupId == groupId && !g.IsDeleted, ct);
            if (group == null) return ServiceResult<bool>.NotFound("Группа не найдена");

            group.IsDeleted = true;
            foreach (var student in group.Students) student.IsDeleted = true;

            await _db.SaveChangesAsync(ct);
            return ServiceResult<bool>.Ok(true);
        }

        private Task<GroupDto> GetDtoAsync(int groupId, CancellationToken ct) =>
            _db.Groups.AsNoTracking().Where(g => g.GroupId == groupId).Select(ToDto).FirstAsync(ct);

        private async Task<string?> ValidateAsync(GroupRequest request, int currentGroupId, CancellationToken ct)
        {
            var name = request.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name)) return "Название группы обязательно";
            if (request.Course < 1 || request.Course > 6) return "Курс должен быть от 1 до 6";
            if (!await _db.Specialtys.AnyAsync(s => s.SpecialtyId == request.SpecialtyId, ct)) return "Специальность не найдена";
            if (await _db.Groups.AnyAsync(g => g.Name == name && !g.IsDeleted && g.GroupId != currentGroupId, ct)) return "Группа с таким названием уже существует";
            return null;
        }
    }
}