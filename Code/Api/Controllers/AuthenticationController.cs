﻿using Application.Common.dto;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SaiBilling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserLoginRequest_DTO userLoginRequest)
        {
            if(userLoginRequest == null)
            {
                throw new ArgumentNullException(nameof(userLoginRequest));
            }
            var token = _authenticationService.AuthenticateUser(userLoginRequest);
            if (token is not null && token.Result is null)
            {
                throw new UnauthorizedAccessException("Please provide correct credentials!!");
            }
            return StatusCode(StatusCodes.Status200OK, token);
        }
    }
}
