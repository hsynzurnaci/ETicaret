using ETicaret.ApplicationAndDomain.Features.Siparisler.Queries.GetSiparisDetay;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static ETicaret.ApplicationAndDomain.Features.Siparisler.Commands.CreateSiparis.SiparisUrunDto;

namespace ETicaret.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SiparislerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SiparislerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/Detay")] 
        public async Task<IActionResult> GetDetay(int id)
        {
            var result = await _mediator.Send(new GetSiparisDetayQuery { SiparisId = id });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSiparisCommandRequest command)
        {
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { Message = "Sipariş başarıyla oluşturuldu." });

            return BadRequest("Sipariş oluşturulurken hata meydana geldi.");
        }
    }
}