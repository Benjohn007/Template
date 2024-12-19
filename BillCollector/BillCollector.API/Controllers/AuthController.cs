using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BillCollector.API.Controllers;
//using BillCollector.Application.Interface;
using BillCollector.Application;
using BillCollector.Application.Dto.Auth;
using BillCollector.Application.Interface;
using BillCollector.CbaProxy;

namespace BillCollector.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;
        private readonly CBAProxy _cbaProxy;

        public AuthController(IUserService userService, CBAProxy cbaProxy)
        {
            _userService = userService;
            _cbaProxy = cbaProxy;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto.Request request)
        {
            var response = await _userService.LoginAsync(request);
            return response.status ? Ok(response) : BadRequest(response);
        }
    }
}
