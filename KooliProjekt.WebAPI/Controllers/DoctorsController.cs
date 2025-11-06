using System.Threading.Tasks;
using KooliProjekt.Application.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class DoctorsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public DoctorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] DoctorsQuery query)
        {
            
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
