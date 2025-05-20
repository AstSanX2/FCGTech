using FCG.Domain.Entities;
using FCG.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace FCG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get() =>
            Ok(await service.GetAllAsync());

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> Get(ObjectId id)
        {
            var user = await service.GetByIdAsync(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            await service.CreateAsync(user);
            return CreatedAtAction(nameof(Get), user);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, User user)
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
