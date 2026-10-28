using KalininaMargaritaKt_31_23.Dto;
using KalininaMargaritaKt_31_23.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NLog.Filters;

namespace KalininaMargaritaKt_31_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupsController : ApiBaseController
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService) => _groupService = groupService;

        [HttpPost("Получить данные неудалённых групп")]
        public async Task<IActionResult> GetGroups([FromBody] CancellationToken ct) =>
            Ok(await _groupService.GetGroupsAsync(ct));


        [HttpPost("Добавить группу")]
        public async Task<IActionResult> AddGroup([FromBody] GroupRequest request, CancellationToken ct) =>
            ToResult(await _groupService.AddGroupAsync(request, ct));

        [HttpPut("Изменить группу по id/{groupId:int}")]
        public async Task<IActionResult> UpdateGroup(int groupId, [FromBody] GroupRequest request, CancellationToken ct) =>
            ToResult(await _groupService.UpdateGroupAsync(groupId, request, ct));

        [HttpDelete("Удалить группу по id/{groupId:int}")]
        public async Task<IActionResult> DeleteGroup(int groupId, CancellationToken ct) =>
            ToResult(await _groupService.DeleteGroupAsync(groupId, ct));
    }
}