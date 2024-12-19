using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillCollector.Application.Dto.Auth;
using BillCollector.Core.Entities;

namespace BillCollector.Application.Interface
{
    public interface IUserService
    {
        User GetByEmail(string email);
        Task<ApiResponseDto<LoginDto.Response>> LoginAsync(LoginDto.Request model);
    }
}
