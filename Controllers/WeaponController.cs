using dotnet_rpg.DTOs.Character;
using dotnet_rpg.DTOs.Weapon;
using dotnet_rpg.Models;
using dotnet_rpg.Service.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _service;
        public WeaponController(IWeaponService service)
        {
            _service = service;
        }

        [HttpPost("AddWeapon")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(await _service.AddWeapon(newWeapon));
        }

    }
}
