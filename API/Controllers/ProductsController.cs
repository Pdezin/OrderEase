using Domain.DTOs.Products;
using Domain.Workflows;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsWorkflow _productsWorkflow;

        public ProductsController(ProductsWorkflow productsWorkflow)
        {
            _productsWorkflow = productsWorkflow;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string term = "",
                                             [FromQuery] bool? active = null,
                                             [FromQuery] int page = 0,
                                             [FromQuery] int pageSize = 10,
                                             [FromQuery] string orderBy = nameof(Product.Id),
                                             [FromQuery] bool orderDesc = false)
        {
            var result = await _productsWorkflow.Get(term, active, page, pageSize, orderBy, orderDesc);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Detail([FromRoute] int id)
        {
            var result = await _productsWorkflow.Detail(id);

            if (_productsWorkflow.IsNotFound)
                return NotFound(_productsWorkflow.Errors);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDTO productDTO)
        {
            var result = await _productsWorkflow.Add(productDTO);

            if (_productsWorkflow.IsValid)
                return StatusCode(StatusCodes.Status201Created, result);

            return BadRequest(_productsWorkflow.Errors);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id,
                                             [FromBody] ProductDTO productDTO)
        {
            await _productsWorkflow.Update(id, productDTO);

            if (_productsWorkflow.IsValid)
                return NoContent();

            if (_productsWorkflow.IsNotFound)
                return NotFound(_productsWorkflow.Errors);

            return BadRequest(_productsWorkflow.Errors);
        }
    }
}
