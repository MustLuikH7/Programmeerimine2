using System.Threading.Tasks;
using KooliProjekt.Application.Features.Users;
using KooliProjekt.Application.Features.InvoiceItems;
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

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetInvoiceItemQuery { ItemId = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveInvoiceItemCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteInvoiceItemCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}
