using AutoMapper;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.DTOs.Fight;
using dotnet_rpg.DTOs.Skill;
using dotnet_rpg.DTOs.Weapon;

namespace dotnet_rpg
{
    public class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto,Character>();
            CreateMap<Weapon,GetWeaponDto>();
            CreateMap<Skill,GetSkillDto>();
            CreateMap<Character, HighScoreDto>();
        }
    }
}
