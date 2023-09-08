using DocuSign.Dto;
using DocuSign.Interfaces;
using DocuSign.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocuSign.Controllers
{
    [Route("[controller]")]
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
            //// change it to not return list
            try
            {
                URI uri = _uriRepository.AddUserUri(userName, body.UriName, body.URL);
                //URIDto  = new URIDto(uri.Name, uri.URL);
                return Ok(uri);

            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpDelete("{URIName}")]
        public IActionResult DeleteUserUri(string URIName, [FromHeader(Name = "userName")] string userName)
        {
            try
            {
                _uriRepository.DeleteUserUri(userName, URIName);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("/URIS")]
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

        [HttpPost("/connect")]
        public IActionResult ConnectUser(string URL, [FromHeader(Name = "userName")] string userName)
        {
            try
            {
                _uriRepository.ConnectUser(userName, URL);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

