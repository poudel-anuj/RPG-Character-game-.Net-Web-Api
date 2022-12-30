using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dotnet_rpg.Controllers.Service.CharacterService;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        //[Route("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            //var data = await _characterService.GetAllCharacter(userId);           
            var data = await _characterService.GetAllCharacter();
            return Ok(data);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
        {
            var data = await _characterService.GetCharacterById(id);
            return Ok(data);
        }

        [HttpPost("AddCharacter")]
        public async Task<ActionResult<List<ServiceResponse<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var data = await _characterService.AddNewCharacter(newCharacter);
            return Ok(data);
        }

        [HttpPut("UpdateCharacter")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var response = await _characterService.UpdateCharacter(updateCharacter);
            if (response == null)
                return NotFound(response);
            return Ok(response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<List<ServiceResponse<GetCharacterDto>>>> Delete(int id)
        {
            var response = await _characterService.DeleteCharacter(id);
            if (response == null)
                return NotFound(response);
            return Ok(response);
        }

        [HttpPost("AddSkill")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            return Ok(await _characterService.AddCharacterSkill(newCharacterSkill));
        }
        
    }
}
