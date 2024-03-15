//using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MISA.CUKCUK.Core.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using System.Runtime.Versioning;
using MISA.CUKCUK.Core.Resources;
using System.Diagnostics;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;

namespace MISA.CUKCUK.WEB082_PMCHIEN.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticateController: ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if(user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    foreach(var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = CreateToken(authClaims);
                    var refreshToken = GenerateRefreshToken();

                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                    await _userManager.UpdateAsync(user);

                    return Ok(
                        new
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            RefreshToken = refreshToken,
                            Expiration = token.ValidTo,
                            ExpirationRefreshToken = user.RefreshTokenExpiryTime
                        });
                }
                //return Unauthorized();
                return StatusCode(401, new
                {
                    devMsg = MISAAuthResource.InvalidUsernameOrPassword,
                    userMsg = MISAAuthResource.InvalidUsernameOrPassword,
                    errorCode = "",
                    moreInfor = "",
                    traceId = ""
                });
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);
                if(userExists != null)
                {
                    return BadRequest( new
                    {
                        devMsg = MISAAuthResource.UserIsExist,
                        userMsg = MISAAuthResource.UserIsExist,
                        errorCode = "",
                        moreInfor = "",
                        traceId = ""
                    });
                }

                ApplicationUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if(!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        devMsg = MISAAuthResource.RegisterFail,
                        userMsg = MISAAuthResource.RegisterFail,
                        errorCode = "",
                        moreInfor = "",
                        traceId = ""
                    });
                }
                return StatusCode(201);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);
                if(userExists != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        devMsg = MISAAuthResource.UserIsExist,
                        userMsg = MISAAuthResource.UserIsExist,
                        errorCode = "",
                        moreInfor = "",
                        traceId = ""
                    });
                }

                ApplicationUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if(!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        devMsg = MISAAuthResource.RegisterFail,
                        userMsg = MISAAuthResource.RegisterFail,
                        errorCode = "",
                        moreInfor = "",
                        traceId = ""
                    });
                }

                // Tạo các quyền Admin và User nếu chưa tồn tại
                if(!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }
                if(!await _roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                }

                if(await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
                if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                }
                return StatusCode(201);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            try
            {
                if(tokenModel is null)
                {
                    return BadRequest(new {
                        devMsg = MISAAuthResource.NullToken,
                        userMsg = MISAAuthResource.NullTokenUser,
                        errorCode = "",
                        moreInfor = "",
                        traceId = ""
                    });
                }

                string? accessToken = tokenModel.AccessToken;
                string? refreshToken = tokenModel.RefreshToken;

                var principal = GetPrincipalFromExpiredToken(accessToken);
                if(principal == null)
                {
                    return BadRequest(new
                    {
                        devMsg = MISAAuthResource.InvalidToken,
                        userMsg = MISAAuthResource.NullTokenUser,
                        errorCode = "",
                        moreInfor = "",
                        traceId = ""
                    });
                }

#pragma warning disable CS8600
#pragma warning disable CS8602
                string username = principal.Identity.Name;
#pragma warning restore CS8602
#pragma warning restore CS8600

                var user = await _userManager.FindByNameAsync(username);

                if(user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return BadRequest(new
                    {
                        devMsg = MISAAuthResource.InvalidToken,
                        userMsg = MISAAuthResource.NullTokenUser,
                        errorCode = "",
                        moreInfor = "",
                        traceId = ""
                    });
                }

                var newAccessToken = CreateToken(principal.Claims.ToList());
                var newRefreshToken = GenerateRefreshToken();

                // Lấy giá trị hết hạn refreshToken mới
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

                // cập nhật refreshToken và thời gian hết hạn mới
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
                await _userManager.UpdateAsync(user);
                return StatusCode(200, new
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                    refreshToken = newRefreshToken,
                    Expiration = newAccessToken.ValidTo,
                    ExpirationRefreshToken = user.RefreshTokenExpiryTime
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if(user == null)
                {
                    return BadRequest(new
                    {
                        devMsg = MISAAuthResource.InvalidUserDevMsg,
                        userMsg = MISAAuthResource.InvalidUserUserMsg,
                        errorCode = "",
                        moreInfor = "",
                        traceId = ""
                    });
                }

                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
                return StatusCode(204);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Hàm tạo Token mới
        /// </summary>
        /// <param name="authClaims">Danh sách Claims</param>
        /// <returns>JwtSecurityToken - token mới</returns>
        /// Created by: PMChien
        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            try
            {
                var authSigningkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningkey, SecurityAlgorithms.HmacSha256)
                    );
                return token;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        /// <summary>
        /// Tạo token mới ngẫu nhiên
        /// </summary>
        /// <returns>Token ngẫu nhiên mới</returns>
        /// Created by: PMChien
        private static string GenerateRefreshToken()
        {
            try
            {
                var randomNumber = new byte[64];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        /// <summary>
        /// Lấy thông tin từ token cũ
        /// </summary>
        /// <param name="token"></param>
        /// <returns>ClaimsPrincipal</returns>
        /// Created by: PMChien
        /// <exception cref="SecurityTokenException"></exception>
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                    ValidateLifetime = false
                };

                var tokenHanlder = new JwtSecurityTokenHandler();
                var principal = tokenHanlder.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if(securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg
                    .Equals(SecurityAlgorithms.HmacSha256, 
                            StringComparison.InvariantCultureIgnoreCase)
                )
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
