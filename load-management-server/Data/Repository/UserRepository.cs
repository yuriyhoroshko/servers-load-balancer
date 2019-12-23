using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DTO;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public static class UserRepository
    {
        public static async Task<UserDto> Register(UserDto userDto)
        {
            LoadManagerContext dbContext = new LoadManagerContext();
            User usr;

            using (dbContext)
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    User user = new User
                    {
                        UserName = userDto.UserName,
                        Password = userDto.Password,
                    };

                    await dbContext.AddAsync<User>(user);
                    await dbContext.SaveChangesAsync();
                    usr = user;
                    transaction.Commit();
                }
            }

            return new UserDto
            {
                UserId = usr.UserId,
                UserName = usr.UserName,
                Password = usr.Password,
            };
        }


        public static async Task<bool> CheckUserCredentials(string userName)
        {
            using (var dbContext = new LoadManagerContext())
            {
                var query = await dbContext.Users.Select(b => new
                {
                    b.UserName,
                }).CountAsync(o => o.UserName == userName);
                return query == 0;
            }
        }

        public static async Task<UserDto> Login(UserDto loginObject)
        {
            LoadManagerContext dbContext = new LoadManagerContext();

            using (dbContext)
            {
                return await dbContext.Users.Select(o => new
                    UserDto
                {
                    UserName = o.UserName,
                    Password = o.Password,
                    UserId = o.UserId
                }).Where(u => u.UserName == loginObject.UserName && u.Password == loginObject.Password).FirstOrDefaultAsync();
            }
        }

        public static async Task<int> GetIdByUserName(string userName)
        {
            LoadManagerContext dbContext = new LoadManagerContext();
            using (dbContext)
            {
                User usr = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                return usr.UserId;
            }
        }
    }
}
