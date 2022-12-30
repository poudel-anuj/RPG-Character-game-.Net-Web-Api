using AutoMapper;
using dotnet_rpg.DTOs.Fight;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace dotnet_rpg.Service.Fight
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public FightService(DataContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            var resp = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);
                if (skill == null)
                {
                    resp.Success = false;
                    resp.Message = $"{attacker.Name} does not know the skills";
                    return resp;
                }

                int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
                damage -= new Random().Next(opponent.Defense);

                if (damage >0)
                    opponent.HitPoints -= damage;
                if (opponent.HitPoints <=0)
                    resp.Message = $"{opponent.Name} has been defeated";

                await _context.SaveChangesAsync();

                resp.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    Damage = damage,
                    AttackerHp = attacker.HitPoints,
                    OpponentHp = opponent.HitPoints
                };

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }


        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var resp = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                damage -= new Random().Next(opponent.Defense);

                if (damage >0)
                    opponent.HitPoints -= damage;
                if (opponent.HitPoints <=0)
                    resp.Message = $"{opponent.Name} has been defeated";

                await _context.SaveChangesAsync();

                resp.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    Damage = damage,
                    AttackerHp = attacker.HitPoints,
                    OpponentHp = opponent.HitPoints
                };

            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
            var resp = new ServiceResponse<FightResultDto>
            {
                Data = new FightResultDto()
            };
            try
            {
                var characters = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => request.CharacterIds.Contains(c.Id)).ToListAsync();

                bool defeated = false;
                while (!defeated)
                {
                    foreach (Character attacker in characters)
                    {
                        var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUSed = string.Empty;
                        bool useWeapon = new Random().Next(2) == 0;
                        if (useWeapon)
                        {
                            attackUSed = attacker.Weapon.Name;
                            damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                            damage -= new Random().Next(opponent.Defense);

                            if (damage >0)
                                opponent.HitPoints -= damage;
                        }
                        else
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUSed = skill.Name;
                            damage = skill.Damage + (new Random().Next(attacker.Intelligence));
                            damage -= new Random().Next(opponent.Defense);

                            if (damage >0)
                                opponent.HitPoints -= damage;

                        }

                        resp.Data.Log
                            .Add($"{attacker.Name} attacks {opponent.Name} using {attackUSed} with {(damage >=0 ? damage : 0)} damage.");

                        if (opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            resp.Data.Log
                                .Add($"{opponent.Name} has been defeated!");
                            resp.Data.Log
                                .Add($"{attacker.Name} wins with {attacker.HitPoints} HP left.");
                            break;
                        }
                           

                    }
                }
                characters.ForEach(c => {
                    c.Fights++;
                    c.HitPoints = 100;
                   
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
        {
            var characters = await _context.Characters
                .Where(c => c.Fights > 0)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c => c.Defeats)
                .ToListAsync();

            var resp = new ServiceResponse<List<HighScoreDto>>
            {
                Data = characters.Select(c => _mapper.Map<HighScoreDto>(c)).ToList()
            };
            return resp;
    }
    }
}
