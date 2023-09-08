using DocuSign.Dto;
using DocuSign.Interfaces;
using DocuSign.Models;
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
            try
            {
                return Ok(_userRepository.GetUser(name));
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpGet("/users")]
        [ProducesResponseType(200, Type = typeof(List<string>))]
        public IActionResult GetUsers()
        {
            try
            {
                return Ok(_userRepository.GetUsers());
            } catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [HttpPost]
		[ProducesResponseType(200, Type = typeof(User))]
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

        [HttpDelete("{name}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteUser(string name)
        {
            try
            {
                _userRepository.DeleteUser(name);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
