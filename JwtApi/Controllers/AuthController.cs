using JwtApi.Data;
using JwtApi.Entities;
using JwtApi.Models;
using JwtApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// [Authorize]
        /// </summary>
        /// <returns></returns>
        [HttpGet("authtest")]
        public async Task<string> authTest()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return "you are authorized";

        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {

            var user = await authService.RegisterAsync(request);
            if (user is null) {
                return BadRequest("user already exists");
            }
            return Ok(user);

        }

        //public class test
        //{
        //    public string UserName { get; set; }
        //    public string Password { get; set; }
        //}

        public class loginVM
        {
            public string Token { get; set; }
            public string UserName { get; set; }
        }

        [HttpPost("login")]
        public async Task<ActionResult<loginVM>> Login(UserDto request)
        {
            var token = await authService.LoginAsync(request);

            if (token is null)
            {
                return BadRequest("invalid username or password");
            }

            var res = new loginVM() { 
                Token = token,
                UserName = request.UserName
            };

            return Ok(res);

        }


        //[HttpPost("login")]
        //public async Task<ActionResult<string>> Login(UserDto request)
        //{
        //    var token = await authService.LoginAsync(request);
        //    if (token is null)
        //    {
        //        return BadRequest("invalid username or password");
        //    }
        //    return Ok(token);

        //}

        //[HttpPost("login")]
        //public async Task<ActionResult<string>> Login([FromForm] string userName, [FromForm] string email, [FromForm] string password)
        //{
        //    var request = new UserDto() { };
        //    var token = await authService.LoginAsync(request);
        //    if (token is null)
        //    {
        //        return BadRequest("invalid username or password");
        //    }
        //    return Ok(token);

        //}
        //[HttpPost("login")]
        //public async Task<ActionResult<string>> Login(string userName, string email, string password)
        //{
        //    var token = "ahsfdlsaj f"; // await authService.LoginAsync(request);
        //    if (token is null)
        //    {
        //        return BadRequest("invalid username or password");
        //    }
        //    return Ok(token);

        //}

        //public class test
        //{
        //    public string field1 { get; set; }
        //    public string field2 { get; set; }
        //}

        //[HttpPost("login")]
        //public async Task<ActionResult<string>> Login(test email)
        //{
        //    var token = "ahsfdlsaj f"; // await authService.LoginAsync(request);
        //    if (token is null)
        //    {
        //        return BadRequest("invalid username or password");
        //    }
        //    return Ok(token);

        //}




    }



}
