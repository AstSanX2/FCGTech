using FCG.API.Domain.DTO.UserDTO;
using FCG.API.Domain.Interfaces.Services;
using FCG.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace FCG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await service.GetAllAsync());
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(ObjectId id)
        {
            var user = await service.GetByIdAsync(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateUserDTO user)
        {
            var createdUser = await service.CreateAsync(user);
            return CreatedAtAction(nameof(Get), createdUser);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, UpdateUserDTO user)
        {
            var existing = await service.GetByIdAsync(ObjectId.Parse(id));
            if (existing is null) return NotFound();

            await service.UpdateAsync(ObjectId.Parse(id), user);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await service.GetByIdAsync(ObjectId.Parse(id));
            if (existing is null) return NotFound();

            await service.DeleteAsync(ObjectId.Parse(id));
            return NoContent();
        }
    }
}
