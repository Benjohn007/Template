using AutoMapper;
using Azure;
using Microsoft.Extensions.Options;
using BillCollector.Core;
using BillCollector.Core.Entities;
using BillCollector.Infrastructure;
using BillCollector.Infrastructure.Repository;
using BillCollector.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillCollector.Application.Dto.Auth;
using BillCollector.Application.Interface;
using BillCollector.Infrastructure.Auth;

namespace BillCollector.Application.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;
        private readonly AppKeys _settings;
        private readonly Infrastructure.Logger.ILog<UserService> _logger;
        private readonly IMapper _mapper;
        private IDapperRepository _dapperRepository;
        private IJwtService _jwtService;

        public UserService(IUnitOfWork unitOfWork, IOptions<AppKeys> options, Infrastructure.Logger.ILog<UserService> logger, IMapper mapper, IDapperRepository dapperRepository, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _settings = options.Value;
            _logger = logger;
            _mapper = mapper;
            _dapperRepository = dapperRepository;
            _jwtService = jwtService;
        }

        public User GetByEmail(string email)
        {
            var result = _unitOfWork.User.GetOne(u => u.Email == email);

            return result;
        }




        //AUTH Methods
        public async Task<ApiResponseDto<LoginDto.Response>> LoginAsync(LoginDto.Request model)
        {
            var result = new ApiResponseDto<LoginDto.Response>();

            var user = _unitOfWork.User.GetOne(u => u.Email == model.email);
            if (user == null)
            {
                result.status = false;
                result.message = "Invalid credentials.";
                return result;
            }

            var passwordHash = PasswordManager.Encrypt(model.password);
            if(user.PasswordHash != passwordHash)
            {
                result.status = false;
                result.message = "Invalid credentials.";
                return result;
            }

            if (!user.EmailVerified)
            {
                result.status = false;
                result.message = "Email account not verified. Check your inbox for verification link.";
                return result;
            }

            if (user.FailedLoginAttempts >= 3)
            {
                result.status = false;
                result.message = "Your account is locked. Please contact administrator.";
                return result;
            }

            var jwt = await _jwtService.GenerateTokenAsync(user);
            var userDto = new UserDto
            {
                Email = user.Email,
                EmailVerified = user.EmailVerified,
                FirstName = user.FirstName,
                LastName = user.LastName,
                LastLoginAt = DateTime.UtcNow,
                PhoneNumber = user.PhoneNumber,
                PhoneVerified = user.PhoneVerified,
                Status = user.Status,
                Role = user.UserRole.Role.Name
            };

            result.data = new LoginDto.Response();
            result.data.user = userDto;
            result.data.token = jwt;
            return result;
        }
    }
}
