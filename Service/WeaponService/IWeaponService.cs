using dotnet_rpg.DTOs.Character;
using dotnet_rpg.DTOs.Weapon;
using dotnet_rpg.Models;

namespace dotnet_rpg.Service.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto addWeaponDto);
    }
}
