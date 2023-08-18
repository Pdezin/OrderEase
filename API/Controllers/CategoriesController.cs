using Domain.DTOs.Categories;
using Domain.Workflows;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        public async Task<IActionResult> Post([FromBody] CategoryDTO categoryDTO)
        {
            await _categoriesWorkflow.Add(categoryDTO);

            if (_categoriesWorkflow.IsValid)
                return Ok();

            return BadRequest(_categoriesWorkflow.Errors);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id,
                                             [FromBody] CategoryDTO categoryDTO)
        {
            await _categoriesWorkflow.Update(id, categoryDTO);

            if (_categoriesWorkflow.IsValid)
                return Ok();

            return BadRequest(_categoriesWorkflow.Errors);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _categoriesWorkflow.Delete(id);

            if (_categoriesWorkflow.IsValid)
                return Ok();

            return BadRequest(_categoriesWorkflow.Errors);
        }
    }
}
