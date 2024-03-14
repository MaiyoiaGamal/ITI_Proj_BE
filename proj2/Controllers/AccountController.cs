using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging.Signing;
using proj2.DTO;
using proj2.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace proj2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public UserManager<ApplicationUser> usermanger { get; }
        public IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this.usermanger = userManager;
            this.config = config;
        }

        //create account
        [HttpPost("Register")]
        public async Task<IActionResult> Registertion(RegisterDTO Userdto)
        {
            if (ModelState.IsValid)
            {
                if (Userdto.Password != Userdto.PasswordConfirmed)
                {
                    return BadRequest("The password and confirm password do not match.");
                }

                ApplicationUser user = new ApplicationUser();
                user.Email = Userdto.Email;
                user.UserName = Userdto.UserName;

                IdentityResult result = await usermanger.CreateAsync(user, Userdto.Password);

                if (result.Succeeded)
                    return Ok("Account successfully added");
                else
                    return BadRequest(result.Errors);
            }

            return BadRequest(ModelState);
        }


        //check account valid login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid == true)
            { 
             ApplicationUser user  = await usermanger.FindByEmailAsync(loginDTO.Email);
                if (user != null)
                {
                    bool found = await usermanger.CheckPasswordAsync(user, loginDTO.Password);
                    if (found )
                    {
                        //claims token 
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, user.Email));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier , user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        // role
                        var roles = await usermanger.GetRolesAsync(user);
                        foreach (var item in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, item));
                        }
                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
                        SigningCredentials signingCred = new SigningCredentials(securityKey , SecurityAlgorithms.HmacSha256);
                        //create token
                        JwtSecurityToken token = new JwtSecurityToken(
                          issuer: config["JWT:ValidIssuer"],//swagger host
                          audience: config["JWT:ValidAudiance"], // anguler url
                          claims: claims,
                          expires : DateTime.Now.AddDays(1),
                          signingCredentials: signingCred
                         );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });


                    }
                }
                return Unauthorized();

            }
            return Unauthorized();
        }
    }
}
