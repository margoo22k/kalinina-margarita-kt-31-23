using Xunit;
using Microsoft.EntityFrameworkCore;
using KalininaMargaritaKt_31_23.Database;
using KalininaMargaritaKt_31_23.Interfaces;
using KalininaMargaritaKt_31_23.Models;
using KalininaMargaritaKt_31_23.Dto;
using System;
using System.Threading.Tasks;

namespace KalininaMargaritaKt_31_23.Tests.IntegrationTests
{
    public class DisciplineServiceTests
    {
        private readonly DbContextOptions<StudentDbContext> _dbContextOptions;

        public DisciplineServiceTests()
        {
            // Создаем настройки для тестовой БД в памяти. 
            // Guid.NewGuid() гарантирует, что каждый тест будет использовать изолированную БД
            _dbContextOptions = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact(DisplayName = "Получение фамилий студентов с двойками: существующая дисциплина возвращает правильные данные")]
        public async Task GetLastNamesStudentsWith2GradesByDisciplineName_ExistingDiscipline_ReturnsCorrectStudents()
        {
            // 1. Arrange (Подготовка данных)
            using var ctx = new StudentDbContext(_dbContextOptions);
            var disciplineService = new DisciplineService(ctx);

            // Добавляем тестовые дисциплины
            var discipline1 = new Discipline { DisciplineId = 1, Name = "мат", IsDeleted = false };
            var discipline2 = new Discipline { DisciplineId = 2, Name = "физ", IsDeleted = false };
            ctx.Disciplines.AddRange(discipline1, discipline2);

            // Добавляем тестовую группу
            var group = new Group { GroupId = 1, Name = "КТ-31-23", Course = 4, SpecialtyId = 1, IsDeleted = false };
            ctx.Groups.Add(group);

            var student1 = new Student { StudentId = 1, FirstName = "Иван", LastName = "Иванов", GroupId = 1, IsDeleted = false };
            var student2 = new Student { StudentId = 2, FirstName = "Иван", LastName = "Петров", GroupId = 1, IsDeleted = false };
            var student3 = new Student { StudentId = 3, FirstName = "Петр", LastName = "Иванов", GroupId = 1, IsDeleted = false };
            var student4 = new Student { StudentId = 4, FirstName = "Петр", LastName = "Сидоров", GroupId = 1, IsDeleted = false };

            ctx.Students.AddRange(student1, student2, student3, student4);

            // Добавляем оценки
            var grade1 = new Grade { GradeId = 1, StudentId = 1, DisciplineId = 1, Value = 2 }; // Иванов Иван, мат, 2 (ПОДХОДИТ)
            var grade2 = new Grade { GradeId = 2, StudentId = 3, DisciplineId = 2, Value = 2 }; // Иванов Петр, физ, 2
            var grade3 = new Grade { GradeId = 3, StudentId = 1, DisciplineId = 2, Value = 5 }; // Иванов Иван, физ, 5
            var grade4 = new Grade { GradeId = 4, StudentId = 4, DisciplineId = 1, Value = 5 }; // Сидоров Петр, мат, 5
            var grade5 = new Grade { GradeId = 5, StudentId = 1, DisciplineId = 1, Value = 5 }; // Иванов Иван, мат, 5

            ctx.Grades.AddRange(grade1, grade2, grade3, grade4, grade5);

            // Сохраняем в тестовую БД
            await ctx.SaveChangesAsync();

            // 2. Act (Вызов тестируемого метода)
            var filter = new DisciplineHas2GradeStudentFilter { DisciplineName = "мат" };
            var result = await disciplineService.GetLastNamesStudentsWith2GradesByDisciplineNameAsync(filter);

            // 3. Assert (Проверка результата)
            Assert.Equal(ServiceStatus.Ok, result.Status);
            Assert.NotNull(result.Data);

            //по дисциплине "мат" двойку получил только student1 (Иванов)
            Assert.Single(result.Data);
            Assert.Contains("Иванов", result.Data);
        }

        [Fact(DisplayName = "Получение фамилий студентов с двойками: несуществующая дисциплина возвращает NotFound")]
        public async Task GetLastNamesStudentsWith2GradesByDisciplineName_NonExistentDiscipline_ReturnsNotFound()
        {
            // 1. Arrange
            using var ctx = new StudentDbContext(_dbContextOptions);
            var disciplineService = new DisciplineService(ctx);

            // 2. Act
            var filter = new DisciplineHas2GradeStudentFilter { DisciplineName = "Несуществующий предмет" };
            var result = await disciplineService.GetLastNamesStudentsWith2GradesByDisciplineNameAsync(filter);

            // 3. Assert
            Assert.Equal(ServiceStatus.NotFound, result.Status);
        }
    }
}