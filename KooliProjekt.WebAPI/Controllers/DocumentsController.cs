using System.Threading.Tasks;
using KooliProjekt.Application.Features.Users;
using KooliProjekt.Application.Features.Documents;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class DocumentsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public DocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] DocumentsQuery query)
        {

            var result = await _mediator.Send(query);

            return Result(result);
        }
        
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetDocumentQuery { DocumentId = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveDocumentCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}
