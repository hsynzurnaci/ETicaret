using ETicaret.ApplicationAndDomain.Features.Raporlar.Queries.GetUrunStokRaporu;
using ETicaret.ApplicationAndDomain.Features.Raporlar.Queries.RunCustomSql;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RaporlarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RaporlarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("OzelRapor")]
        public async Task<IActionResult> Calistir([FromBody] RunCustomSqlQuery query)
        {
            try
            {
                var sonuc = await _mediator.Send(query);
                return Ok(sonuc);
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Sorgu Hatası: {ex.Message}");
            }
        }


        [HttpGet("UrunStok")]
        public async Task<IActionResult> GetUrunStokRaporu()
        {
            var rapor = await _mediator.Send(new GetUrunStokRaporuQuery());
            return Ok(rapor);
        }
    }
}