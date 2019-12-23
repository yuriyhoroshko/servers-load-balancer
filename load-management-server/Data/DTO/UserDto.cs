using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }
    }
}
