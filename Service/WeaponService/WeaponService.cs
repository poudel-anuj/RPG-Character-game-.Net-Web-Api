using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.DTOs.Weapon;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dotnet_rpg.Service.WeaponService
{
    public class WeaponService : IWeaponService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public WeaponService(DataContext context,
            IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context=context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            ServiceResponse<GetCharacterDto> resp = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                    c.User.Id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                if (character == null)
                {
                    resp.Success = false;
                    resp.Message = "Character Not found";
                    return resp;
                }

                Weapon weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };
                _context.Add(weapon);
                await _context.SaveChangesAsync();
                resp.Data = _mapper.Map<GetCharacterDto>(character);
                resp.Message = "Weapons saved.";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }
            return resp;

        }
    }
}
