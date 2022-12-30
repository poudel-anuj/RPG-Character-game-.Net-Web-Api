using dotnet_rpg.DTOs.Fight;

namespace dotnet_rpg.Service.Fight
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skillAttack);
        Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto skillAttack);
        Task<ServiceResponse<List<HighScoreDto>>> GetHighScore();
        
    }
}
