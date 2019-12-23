using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.DTO;

namespace Business
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(UserDto userBllDto);

        Task<UserDto> LoginUserAsync(UserDto loginObject);

    }
}
