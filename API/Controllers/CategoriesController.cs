using Domain.DTOs;
using Domain.Workflows;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesWorkflow _categoriesWorkflow;

        public CategoriesController(CategoriesWorkflow categoriesWorkflow)
        {
            _categoriesWorkflow = categoriesWorkflow;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name = "",
                                             [FromQuery] int page = 0,
                                             [FromQuery] int pageSize = 5)
        {
            var result = await _categoriesWorkflow.Get(name, page, pageSize);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryDTO categoryDto)
        {
            await _categoriesWorkflow.Add(categoryDto);

            if (_categoriesWorkflow.IsValid)
                return Ok();

            return BadRequest(_categoriesWorkflow.Errors);
        }
    }
}
