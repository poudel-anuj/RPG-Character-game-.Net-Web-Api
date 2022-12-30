using dotnet_rpg.DTOs.Fight;
using dotnet_rpg.Service.Fight;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;
        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("WeaponAttack")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto attack)
        {
            return Ok(await _fightService.WeaponAttack(attack));
        }
        [HttpPost("SkillAttack")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto attack)
        {
            return Ok(await _fightService.SkillAttack(attack));
        }

        [HttpPost("Fight")]
        public async Task<ActionResult<ServiceResponse<FightResultDto>>> Fight(FightRequestDto attack)
        {
            return Ok(await _fightService.Fight(attack));
        }

        [HttpPost("HighScore")]
        public async Task<ActionResult<ServiceResponse<List<HighScoreDto>>>> GetHighScore()
        {
            return Ok(await _fightService.GetHighScore());
        }
    }
}
