using dotnet_rpg.DTOs.Character;
using dotnet_rpg.DTOs.Skill;
using dotnet_rpg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Controllers.Service.CharacterService
{
    public interface ICharacterService
    {
        //Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter(int userId);
        //also we can get userId by using ContextAccessor
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter();
        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);
        Task<ServiceResponse<List<GetCharacterDto>>> AddNewCharacter(AddCharacterDto character);
        Task<ServiceResponse<List<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto character);
        Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id);
        Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill); 
    }
}
 