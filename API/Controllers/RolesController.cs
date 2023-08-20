using Domain.DTOs.Roles;
using Domain.Workflows;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RolesWorkflow _rolesWorkflow;

        public RolesController(RolesWorkflow rolesWorkflow)
        {
            _rolesWorkflow = rolesWorkflow;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string name = "",
                                             [FromQuery] int page = 0,
                                             [FromQuery] int pageSize = 5)
        {
            var result = await _rolesWorkflow.Get(name, page, pageSize);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoleDTO roleDTO)
        {
            var result = await _rolesWorkflow.Add(roleDTO);

            if (_rolesWorkflow.IsValid)
                return StatusCode(StatusCodes.Status201Created, result);

            return BadRequest(_rolesWorkflow.Errors);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id,
                                             [FromBody] RoleDTO roleDTO)
        {
            await _rolesWorkflow.Update(id, roleDTO);

            if (_rolesWorkflow.IsValid)
                return NoContent();

            if (_rolesWorkflow.IsNotFound)
                return NotFound(_rolesWorkflow.Errors);

            return BadRequest(_rolesWorkflow.Errors);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _rolesWorkflow.Delete(id);

            if (_rolesWorkflow.IsValid)
                return NoContent();

            if (_rolesWorkflow.IsNotFound)
                return NotFound(_rolesWorkflow.Errors);

            return BadRequest(_rolesWorkflow.Errors);
        }
    }
}
