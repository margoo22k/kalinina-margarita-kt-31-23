using KalininaMargaritaKt_31_23.Dto;
using KalininaMargaritaKt_31_23.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KalininaMargaritaKt_31_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DisciplinesController : ApiBaseController
    {
        private readonly IDisciplineService _disciplineService;

        public DisciplinesController(IDisciplineService disciplineService) => _disciplineService = disciplineService;

        [HttpPost("По названию дисциплины получить список фамилий студентов, у которых по этой дисциплине есть двойки")]
        public async Task<IActionResult> GetLastNamesStudentsWith2GradesByDisciplineName([FromBody] DisciplineHas2GradeStudentFilter filter, CancellationToken ct) =>
            Ok(await _disciplineService.GetLastNamesStudentsWith2GradesByDisciplineNameAsync(filter, ct));


        [HttpPost("Получить данные неудалённых дисциплин")]
        public async Task<IActionResult> GetDisciplines([FromBody] CancellationToken ct) =>
            Ok(await _disciplineService.GetDisciplinesAsync(ct));

        [HttpPost("Добавить дисциплину")]
        public async Task<IActionResult> AddDiscipline([FromBody] DisciplineRequest request, CancellationToken ct) =>
            ToResult(await _disciplineService.AddDisciplineAsync(request, ct));

        [HttpPut("Изменить дисциплину по id/{disciplineId:int}")]
        public async Task<IActionResult> UpdateDiscipline(int disciplineId, [FromBody] DisciplineRequest request, CancellationToken ct) =>
            ToResult(await _disciplineService.UpdateDisciplineAsync(disciplineId, request, ct));

        [HttpDelete("Удалить дисциплину по id/{disciplineId:int}")]
        public async Task<IActionResult> DeleteDiscipline(int disciplineId, CancellationToken ct) =>
            ToResult(await _disciplineService.DeleteDisciplineAsync(disciplineId, ct));
    }
}