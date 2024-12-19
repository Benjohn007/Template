using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillCollector.Infrastructure.Auth;

namespace BillCollector.Application.Dto.Auth
{
    public class LoginDto
    {
        public class Request
        {
            /// <summary>
            /// 
            /// </summary>
            public string email { get; set; } = "admin@billcollector.com";
            public string password { get; set; } = "Admin@123";
        }

        public class Response
        {
            public TokenResponse token { get; set; }
            public UserDto user { get; set; }
        }
    }
}
