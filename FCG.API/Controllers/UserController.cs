using FCG.API.Domain.DTO.UsersDTO;
using FCG.API.Domain.Enums;
using FCG.API.Domain.Interfaces.Services;
using FCG.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace FCG.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = nameof(UserRole.Admin))]
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

        [HttpPost("admin")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> CreateAdmin(CreateUserAdminDTO user)
        {
            var createdUser = await service.CreateAsync(user);
            return CreatedAtAction(nameof(Get), createdUser);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, UpdateUserDTO user)
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (userId == null) return Unauthorized();

            if (userId != id)
            {
                var isAdmin = User.IsInRole(UserRole.Admin.ToString());
                if (!isAdmin) return Unauthorized();
            }

            var existing = await service.GetByIdAsync(ObjectId.Parse(id));
            if (existing is null) return NotFound();

            await service.UpdateAsync(ObjectId.Parse(id), user);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<IActionResult> Delete(string id)
        { 
            var existing = await service.GetByIdAsync(ObjectId.Parse(id));
            if (existing is null) return NotFound();

            await service.DeleteAsync(ObjectId.Parse(id));
            return NoContent();
        }
    }
}
