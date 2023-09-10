using DocuSign.Dto;
using DocuSign.Interfaces;
using DocuSign.Models;
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
        public IActionResult AddUserUri([FromBody] AddURIDto uri, [FromHeader(Name = "userName")] string userName)
        {
            try
            {
                _uriRepository.AddUserUri(userName, uri.UriName, uri.Url);
                return Ok(new AddUserUriResponse(uri.Url, userName));

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

        [HttpGet("/uri")]
        public IActionResult GetUserUris([FromHeader(Name = "userName")] string userName)
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
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}