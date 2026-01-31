using ETicaret.ApplicationAndDomain.Features.PiyasaAnalizi.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ETicaret.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PiyasaAnaliziController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PiyasaAnaliziController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Ekle")]
        public async Task<IActionResult> Ekle([FromBody] CreateAnalizCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}