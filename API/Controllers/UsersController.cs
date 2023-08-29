using Domain.DTOs.Users;
using Domain.Workflows;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UsersWorkflow _usersWorkflow;

        public UsersController(UsersWorkflow usersWorkflow)
        {
            _usersWorkflow = usersWorkflow;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string term = "",
                                             [FromQuery] bool? active = null,
                                             [FromQuery] int page = 0,
                                             [FromQuery] int pageSize = 10)
        {
            var result = await _usersWorkflow.Get(term, active, page, pageSize);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Detail([FromRoute] int id)
        {
            var result = await _usersWorkflow.Detail(id);

            if (_usersWorkflow.IsNotFound)
                return NotFound(_usersWorkflow.Errors);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDTO userDTO)
        {
            var result = await _usersWorkflow.Add(userDTO);

            if (_usersWorkflow.IsValid)
                return StatusCode(StatusCodes.Status201Created, result);

            return BadRequest(_usersWorkflow.Errors);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id,
                                             [FromBody] UserDTO userDTO)
        {
            await _usersWorkflow.Update(id, userDTO);

            if (_usersWorkflow.IsValid)
                return NoContent();

            if (_usersWorkflow.IsNotFound)
                return NotFound(_usersWorkflow.Errors);

            return BadRequest(_usersWorkflow.Errors);
        }
    }
}
