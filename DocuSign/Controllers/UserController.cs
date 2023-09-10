using DocuSign.Dto;
using Microsoft.AspNetCore.Mvc;
using DocuSign.Interfaces;

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
            return Ok(_userRepository.GetUser(userName));
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userRepository.GetUsers()); 
        }

        [HttpPost]
		public IActionResult CreateUser([FromBody] UserDto user)
		{
            return Ok(_userRepository.CreateUser(user.Name, user.LastName, user.Email));
        }

        [HttpDelete("{userName}")]
        public IActionResult DeleteUser([FromRoute] string userName)
        {
            _userRepository.DeleteUser(userName);
            return Ok();
        }
    }
}
