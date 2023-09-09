using System.Dynamic;
using DocuSign.Dto;
//using DocuSign.Interfaces;
using Domain.Interfaces;

//using DocuSign.Models;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocuSign.Controllers
{
    [Route("/uri")]
    [ApiController]
    public class URIController : Controller
	{
        private readonly IURIRepository _uriRepository;

        public URIController(IURIRepository uriRepository)
        {
            _uriRepository = uriRepository;
        }

        [HttpPost]
        public IActionResult AddUserUri([FromBody] AddURIDto body, [FromHeader(Name = "userName")] string userName)
        {
            try
            {
                _uriRepository.AddUserUri(userName, body.UriName, body.Url);
                AddUserUriResponse response = new AddUserUriResponse(body.Url, userName);

                return Ok(response);

            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpDelete("{uriName}")]
        public IActionResult DeleteUserUri([FromRoute] string uriName, [FromHeader(Name = "userName")] string userName)
        {
            try
            {
                _uriRepository.DeleteUserUri(userName, uriName);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("/uris")]
        public IActionResult DeleteUserUri([FromHeader(Name = "userName")] string userName)
        {
            try
            {
                return Ok(_uriRepository.GetUserUris(userName));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{url}")]
        public IActionResult ConnectUser([FromRoute] string url, [FromHeader(Name = "userName")] string userName)
        {
            try
            {
                _uriRepository.ConnectUser(userName, url);
                return Ok(url);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}