using System;
using System.Xml.Linq;
using DocuSign.Dto;
using DocuSign.Interfaces;
using DocuSign.Models;
using DocuSign.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DocuSign.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UserController : Controller
	{
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
		{
            _userRepository = userRepository;
        }

		[HttpGet("{name}")]
		[ProducesResponseType(200, Type = typeof(User))]
		public IActionResult GetUser(string name)
		{
			User user = _userRepository.GetUser(name);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return Ok(user);
        }

        [HttpGet("/users")]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public IActionResult GetUsers()
        {
            List<string> userNames = _userRepository.GetUsers();

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            return Ok(userNames);
        }

        [HttpPost]
		[ProducesResponseType(200, Type = typeof(User))]
		[ProducesResponseType(400)]
		//hanDALe so that urllist will not appear in request body
		public IActionResult CreateUser([FromBody] UserDto user)
		{
			if(user == null)
			{
				return BadRequest(ModelState);
			}

			// make validation if user already exsits and return suitable response

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			//if (!_userRepository.CreateUser(user))
			//{
			//	ModelState.AddModelError("", "Went wrong"); 
			//	return StatusCode(500, ModelState);
			//}
			User u = _userRepository.CreateUser(user.Name, user.LastName, user.Email);
			return Ok(u);
        }

        [HttpDelete("{name}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteUser(string name)
        {
            _userRepository.DeleteUser(name);

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            return Ok();
        }
    }
}
