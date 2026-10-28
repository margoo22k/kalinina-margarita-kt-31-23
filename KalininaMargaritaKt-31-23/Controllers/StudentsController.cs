using KalininaMargaritaKt_31_23.Dto;
using KalininaMargaritaKt_31_23.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KalininaMargaritaKt_31_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ApiBaseController
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService) => _studentService = studentService;

        [HttpPost("Получить данные неудалённых студенов")]
        public async Task<IActionResult> GetStudents([FromBody] CancellationToken ct) =>
            Ok(await _studentService.GetStudentsAsync(ct));

        [HttpPost("Добавить студента")]
        public async Task<IActionResult> AddStudent([FromBody] StudentRequest request, CancellationToken ct) =>
            ToResult(await _studentService.AddStudentAsync(request, ct));

        [HttpPut("Изменить студента по id/{studentId:int}")]
        public async Task<IActionResult> UpdateStudent(int studentId, [FromBody] StudentRequest request, CancellationToken ct) =>
            ToResult(await _studentService.UpdateStudentAsync(studentId, request, ct));

        [HttpDelete("Удалить студента по id/{studentId:int}")]
        public async Task<IActionResult> DeleteStudent(int studentId, CancellationToken ct) =>
            ToResult(await _studentService.DeleteStudentAsync(studentId, ct));
    }
}