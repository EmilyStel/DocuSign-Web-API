﻿using DocuSign.Dto;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;

namespace DocuSign.Controllers
{
	[Route("/user")]
	[ApiController]
	public class UserController : Controller
	{
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
		{
            _userRepository = userRepository;
        }

		[HttpGet("{userName}")]
		public IActionResult GetUser([FromRoute] string userName)
		{
            try
            {
                return Ok(_userRepository.GetUser(userName));
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpGet("/users")]
        public IActionResult GetUsers()
        {
            try
            {
                return Ok(_userRepository.GetUsers());
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }

            //throw new InvalidOperationException("WTF");

        }

        [HttpPost]
		public IActionResult CreateUser([FromBody] UserDto user)
		{
			try
			{
                return Ok(_userRepository.CreateUser(user.Name, user.LastName, user.Email));
            } catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpDelete("{userName}")]
        public IActionResult DeleteUser([FromRoute] string userName)
        {
            try
            {
                _userRepository.DeleteUser(userName);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
