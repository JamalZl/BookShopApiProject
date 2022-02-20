using AutoMapper;
using BookShopApi.Apps.UserApi.DTOs;
using BookShopApi.Data.Entities;
using BookShopApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShopApi.Apps.UserApi.Controllers
{
    [ApiExplorerSettings(GroupName = "user_v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;

        public AccountsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration configuration, IJwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
            _jwtService = jwtService;
        }
        //[HttpGet("roles")]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    var result = await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    result = await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    result = await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    return Ok();
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            AppUser user = await _userManager.FindByNameAsync(registerDto.UserName);

            if (user != null)
                return StatusCode(409);

            user = new AppUser
            {
                UserName = registerDto.UserName,
                FullName = registerDto.FullName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded)
                return BadRequest(result.Errors);


            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {

            AppUser user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
                return NotFound();

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            string tokenStr = _jwtService.Generate(user, roles, _configuration);

            return Ok(new { token = tokenStr });
        }
        [HttpGet("")]
        [Authorize(Roles ="Member")]
        public async Task<IActionResult> Get()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            AccountGetDto accountDto = _mapper.Map<AccountGetDto>(user);

            return Ok(accountDto);


        }
    }
}
