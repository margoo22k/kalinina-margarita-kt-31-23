using KalininaMargaritaKt_31_23.Dto;
using KalininaMargaritaKt_31_23.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KalininaMargaritaKt_31_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpecialtiesController : ApiBaseController
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtiesController(ISpecialtyService specialtyService) => _specialtyService = specialtyService;

        [HttpPost("Получить данные специальностей")]
        public async Task<IActionResult> GetSpecialys([FromBody] CancellationToken ct) =>
            Ok(await _specialtyService.GetSpecialtysAsync(ct));

        [HttpPost("Добавить специальность")]
        public async Task<IActionResult> AddSpecialty([FromBody] SpecialtyRequest request, CancellationToken ct) =>
            ToResult(await _specialtyService.AddSpecialtyAsync(request, ct));

        [HttpPut("Изменить специальность по id/{specialtyId:int}")]
        public async Task<IActionResult> UpdateSpecialty(int specialtyId, [FromBody] SpecialtyRequest request, CancellationToken ct) =>
            ToResult(await _specialtyService.UpdateSpecialtyAsync(specialtyId, request, ct));
    }
}