using ETicaret.ApplicationAndDomain.Features.Kategoriler.Queries.GetAllKategoriler;
using ETicaret.ApplicationAndDomain.Features.Urunler.Commands.ApplyDiscount;
using ETicaret.ApplicationAndDomain.Features.Urunler.Commands.CreateUrun;
using ETicaret.ApplicationAndDomain.Features.Urunler.Commands.UpdateFiyatlar;
using ETicaret.ApplicationAndDomain.Features.Urunler.Commands.UpdateUrun;
using ETicaret.ApplicationAndDomain.Features.Urunler.Queries.GetOrtalamaFiyat;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UrunlerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UrunlerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Filtrele")] 
        public async Task<IActionResult> Filtrele([FromQuery] GetUrunlerByFiltreQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("Kategoriler")] 
        public async Task<IActionResult> GetKategoriler()
        {
         
            var result = await _mediator.Send(new GetAllKategorilerQuery());
            return Ok(result);
        }
        [HttpGet("OrtalamaFiyat")]
        public async Task<IActionResult> GetOrtalamaFiyat([FromQuery] string kategori)
        {
            var result = await _mediator.Send(new GetOrtalamaFiyatQuery { Kategori = kategori });
            return Ok(result);
        }

        [HttpPost("Ekle")]
        public async Task<IActionResult> Ekle([FromBody] CreateUrunCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("Guncelle")]
        public async Task<IActionResult> Guncelle([FromBody] UpdateUrunCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        [HttpPost("FiyatlariGuncelle")]
        public async Task<IActionResult> UpdateFiyatlar()
        {

            var message = await _mediator.Send(new UpdateFiyatlarCommand());
            return Ok(new { Message = message });
        }

        [HttpPost()]
        public async Task<IActionResult> KategoriIndirimi([FromBody] ApplyDiscountCommand command)
        {
            await _mediator.Send(command);
            return Ok(true);
        }
    }
}