using DocuSign.Dto;
using Microsoft.AspNetCore.Mvc;
using DocuSign.Interfaces;
using Domain.Exceptions;

namespace DocuSign.Controllers
{
	[Route("/user")]
	[ApiController]
	public class UserController : Controller
	{
        private readonly IUserRepository _userRepository;
        public ILogger _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
		{
            _userRepository = userRepository;
            _logger = logger;
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

        [HttpGet("/user")]
        public IActionResult GetUsers()
        {
            return Ok(_userRepository.GetUsers()); 
        }

        [HttpPost]
		public IActionResult CreateUser([FromBody] UserDto user)
		{
            try
            {
                return Ok(_userRepository.CreateUser(user.Name, user.LastName, user.Email));
            }
            catch (Exception e)
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
