using System.Threading.Tasks;
using KooliProjekt.Application.Features.Users;
using KooliProjekt.Application.Features.DoctorSchedules;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace KooliProjekt.WebAPI.Controllers
{
    public class DoctorSchedulesController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public DoctorSchedulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] DoctorSchedulesQuery query)
        {

            var result = await _mediator.Send(query);

            return Result(result);
        }
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetDoctorScheduleQuery { ScheduleId = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveDoctorScheduleCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteDoctorScheduleCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}
