using dotnet_rpg.DTOs.User;
using dotnet_rpg.Models;
using dotnet_rpg.Service.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _auth;
        public AuthenticationController(IAuthentication auth)
        {
            _auth = auth;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto reg)
        {
            var response = await _auth.Register(
                new User { UserName = reg.UserName}, reg.Password
                );
            if(!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto userDto)
        {
            var response = await _auth.Login(userDto.UserName, userDto.Password);
            if (!response.Success)
            {
                return NotFound(response.Message);
            }
            return Ok(response);



        }
    }
}
