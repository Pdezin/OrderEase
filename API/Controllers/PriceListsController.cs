using Domain.DTOs.PriceLists;
using Domain.Workflows;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PriceListsController : ControllerBase
    {
        private readonly PriceListsWorkflow _priceListsWorkflow;

        public PriceListsController(PriceListsWorkflow priceListsWorkflow)
        {
            _priceListsWorkflow = priceListsWorkflow;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string term = "",
                                             [FromQuery] bool? active = null,
                                             [FromQuery] int page = 0,
                                             [FromQuery] int pageSize = 10)
        {
            var result = await _priceListsWorkflow.Get(term, active, page, pageSize);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PriceListsDTO priceListsDTO)
        {
            var result = await _priceListsWorkflow.Add(priceListsDTO);

            if (_priceListsWorkflow.IsValid)
                return StatusCode(StatusCodes.Status201Created, result);

            return BadRequest(_priceListsWorkflow.Errors);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id,
                                             [FromBody] PriceListsDTO priceListsDTO)
        {
            await _priceListsWorkflow.Update(id, priceListsDTO);

            if (_priceListsWorkflow.IsValid)
                return NoContent();

            if (_priceListsWorkflow.IsNotFound)
                return NotFound(_priceListsWorkflow.Errors);

            return BadRequest(_priceListsWorkflow.Errors);
        }
    }
}
