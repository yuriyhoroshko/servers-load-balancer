using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data.DTO;
using Data.Repository;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


namespace Business
{
    public class UserService : IUserService
    {
        public async Task<UserDto> RegisterUserAsync(UserDto userBllDto)
        {

            return await Data.Repository.UserRepository.CheckUserCredentials(userBllDto.UserName) ?
                await Data.Repository.UserRepository.Register(userBllDto) :
                null;
        }

        public async Task<UserDto> LoginUserAsync(UserDto loginObject)
        {
            try
            {
                UserDto loginedUser = await UserRepository.Login(loginObject);
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginedUser.UserName)
                };

                DateTime now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                loginedUser.Token = new JwtSecurityTokenHandler().WriteToken(jwt);

                return loginedUser;
            }
            catch (System.Exception e)
            {
                return null;
            }
        }
    }
}
