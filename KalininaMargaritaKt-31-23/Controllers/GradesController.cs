using KalininaMargaritaKt_31_23.Dto;
using KalininaMargaritaKt_31_23.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KalininaMargaritaKt_31_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GradesController : ApiBaseController
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService) => _gradeService = gradeService;


        [HttpPost("Получить данные оценок")]
        public async Task<IActionResult> GetGradess([FromBody] CancellationToken ct) =>
            Ok(await _gradeService.GetGradesAsync(ct));

        [HttpPost("Средний балл по предмету в группе")]
        public async Task<IActionResult> GetGroupDisciplineAverage([FromBody] GroupDisciplineAverageFilter filter, CancellationToken ct) =>
            ToResult(await _gradeService.GetGroupDisciplineAverageAsync(filter, ct));

        [HttpPost("Оценки студента")]
        public async Task<IActionResult> GetStudentGrades([FromBody] StudentGradesFilter filter, CancellationToken ct) =>
            ToResult(await _gradeService.GetStudentGradesAsync(filter, ct));

        [HttpPost("Средний балл по курсу")]
        public async Task<IActionResult> GetYearAverage([FromBody] YearAverageFilter filter, CancellationToken ct) =>
            ToResult(await _gradeService.GetYearAverageAsync(filter, ct));

        [HttpPost("Добавить оценку")]
        public async Task<IActionResult> AddGrade([FromBody] AddGradeRequest request, CancellationToken ct) =>
            ToResult(await _gradeService.AddGradeAsync(request, ct));

        [HttpPut("Изменить оценку по id/{gradeId:int}")]
        public async Task<IActionResult> UpdateGrade(int gradeId, [FromBody] UpdateGradeRequest request, CancellationToken ct) =>
            ToResult(await _gradeService.UpdateGradeAsync(gradeId, request, ct));
    }
}