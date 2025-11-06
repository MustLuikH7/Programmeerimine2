using System.Threading.Tasks;
using KooliProjekt.Application.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class InvoiceItemsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public InvoiceItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] InvoiceItemsQuery query)
        {
            
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
