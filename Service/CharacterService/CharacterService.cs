using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dotnet_rpg.Controllers.Service.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor contextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddNewCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);

            //get current user Id
            character.User = await _context.Users.FirstOrDefaultAsync(c => c.Id == GetUserId());
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            //get all characters belonging to current user;
            var userChar = await _context.Characters.Where(c => c.User.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = userChar.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

            //get all charactes without user filter process
            //serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }


        //public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter(int userId)
        //{
        //    var serviceResp = new ServiceResponse<List<GetCharacterDto>>();
        //    var dbCharacters = await _context.Characters.Where(c => c.Id == userId).ToListAsync();
        //    serviceResp.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        //    return serviceResp;
        //}

        //Now we wil get userId using contextAccessor

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
        {
            var serviceResp = new ServiceResponse<List<GetCharacterDto>>();
            var dbChars = await _context.Characters.Where(c => c.User.Id == GetUserId()).ToListAsync();
            serviceResp.Data = dbChars.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResp;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serResp = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id  && c.User.Id == GetUserId());
            serResp.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serResp;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = await _context.Characters
                    //here we can include character and for that we use include
                    .Include(c => c.User)
                    .FirstAsync(c => c.Id == updateCharacter.Id);
                if (character.User.Id !=null)
                {
                    character.Name = updateCharacter.Name;
                    character.HitPoints = updateCharacter.HitPoints;
                    character.Strength = updateCharacter.Strength;
                    character.Defense = updateCharacter.Defense;
                    character.Intelligence = updateCharacter.Intelligence;
                    character.Class = updateCharacter.Class;
                    _context.Characters.Update(character);
                    await _context.SaveChangesAsync();
                    var chars = await _context.Characters.Where(c => c.User.Id == GetUserId()).ToListAsync();
                    //response.Data = _mapper.Map<GetCharacterDto>(character);
                    response.Data =  chars.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "User id not found in character table";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }


        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var resp = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character chars = await _context.Characters.FirstAsync(c => c.Id == id && c.User.Id == GetUserId());
                if (chars !=null)
                {
                    _context.Characters.Remove(chars);
                    await _context.SaveChangesAsync();
                    resp.Data = await _context.Characters
                    .Where(c => c.User.Id == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
                    resp.Message = "Deleted Successfully";
                }
                else
                {
                    resp.Success = false;
                    resp.Message = "Character not found.Unable to the operation";
                }


            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var resp = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                    .Include(c =>c.Weapon)
                    .Include(c =>c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId
                    && c.User.Id == GetUserId());

                if(character == null)
                {
                    resp.Success = false;
                    resp.Message = "Character not found";
                    return resp;
                }

                var skills = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if(skills == null)
                {
                    resp.Success = false;
                    resp.Message = "Skills not found";
                    return resp;
                }
                character.Skills.Add(skills);
                await _context.SaveChangesAsync();
                resp.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
            }
            return resp;
        }
    }
}
