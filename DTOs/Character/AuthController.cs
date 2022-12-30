using dotnet_rpg.DTOs.User;
using dotnet_rpg.Models;
using dotnet_rpg.Service.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.DTOs.Character
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthentication _auth;
        public AuthController(IAuthentication auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var resp = await _auth.Register(
                new Models.User { UserName = request.UserName }, request.Password
                );
            if (resp.Success)
            {
                return Ok(resp);
            }
            else
                return BadRequest(resp);
        }

    }
}
