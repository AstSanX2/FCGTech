using FCG.API.Domain.DTO.GameDTO;
using FCG.API.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace FCG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController(IGameService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await service.GetAllAsync());
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(ObjectId id)
        {
            var game = await service.GetByIdAsync(id);
            return game is null ? NotFound() : Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateGameDTO game)
        {
            var createdGame = await service.CreateAsync(game);
            return CreatedAtAction(nameof(Get), createdGame);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, UpdateGameDTO game)
        {
            var existingGame = await service.GetByIdAsync(ObjectId.Parse(id));
            if (existingGame is null) return NotFound();

            await service.UpdateAsync(ObjectId.Parse(id), game);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingGame = await service.GetByIdAsync(ObjectId.Parse(id));
            if (existingGame is null) return NotFound();

            await service.DeleteAsync(ObjectId.Parse(id));
            return NoContent();
        }
    }
}
