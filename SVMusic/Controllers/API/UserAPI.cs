using Google.Apis.YouTube.v3;
using Libs;
using Libs.Enity.UserCustom;
using Libs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SVMusic.Models;
using SVMusic.Models.User;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SVMusic.Controllers.API
{
    [Route("API/[controller]")]
    [ApiController]
    public class UserAPI : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext applicationDbContext;
        YoutubeServices _youtubeServices;
        private RoleManager<IdentityRole> _roleManager
        {
            get;
        }
        private UserServices userService;

        public UserAPI(UserServices userService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext applicationDbContext, YoutubeServices youtubeServices)
        {
            this.userService = userService;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
            this.applicationDbContext = applicationDbContext;
            this._youtubeServices = youtubeServices;
        }

        [HttpPost]
        [Route("addvideo")]
        public async Task<IActionResult> AddVideo(string username,string Id,string plname)
        {
            var user = await _userManager.FindByNameAsync(username);
            var playlist = applicationDbContext.userPlayLists.Where(x => x.UserId == user.Id && x.PlayListName == plname).Select(x=>x).FirstOrDefault();
            var videodetail = applicationDbContext.youtubePlayLists.Where(x=>x.VideoId==Id && x.PlayListId == playlist.PlayListId).Select(x=>x).SingleOrDefault();
            if (videodetail == null && user != null)
            {
                await _youtubeServices.Addvideo(Id, playlist.PlayListId);
                return Ok(new Models.Response { Status = true, Message = "Add success" });
            }
            return Ok(new Models.Response { Status = false, Message = "Add fail" });
        }

        [HttpPost]
        [Route("addplaylist")]
        public async Task<IActionResult> AddPlaylist(string username,string plname)
        {
            var user = await _userManager.FindByNameAsync(username);
            var playlist = applicationDbContext.userPlayLists.Where(x => x.UserId == user.Id && x.PlayListName == plname).Select(x => x).FirstOrDefault();
            if (playlist == null && user != null)
            {
                UserPlayList userPlayList = new UserPlayList()
                {
                    PlayListId = new Guid(),
                    PlayListName = plname,
                    UserId = user.Id,
                };
                userService.insertPlaylist(userPlayList);
                return Ok(new Models.Response { Status = false, Message = "Add success" });
            }
            return Ok(new Models.Response { Status = false, Message = "Add fail" });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new IdentityUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            User user1 = new User()
            {
                UserId = user.Id,
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
            };
            userService.insertUser(user1);
            return Ok(new Models.Response { Status = true , Message = "Welcome" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> SignIn([FromBody] LoginModel login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password) && (await _signInManager.PasswordSignInAsync(user, login.Password, false, false)).Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", user.Id)
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    expires: DateTime.UtcNow.AddSeconds(300),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                return Ok(new LoginResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Status = true
                });
            }
            return Unauthorized();
        }
       

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Ok(new Models.Response { Message = "Logout success!", Status = true });
        }
        #region

        #endregion

    }

}