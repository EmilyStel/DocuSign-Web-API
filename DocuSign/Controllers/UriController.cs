using System.Dynamic;
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
            try
            {
                URI uri = _uriRepository.AddUserUri(userName, body.UriName, body.URL);
                //return Ok(c);


                dynamic json = new ExpandoObject();
                //json.Result = 200;
                //json.Message = "success";
                json.Name = uri.Name;
                json.URL = uri.URL;

                //return new JsonResult(json);

                return Ok(json);


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

        [HttpGet("/list")]
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

        [HttpPost("/{url}")]
        public IActionResult ConnectUser(string url, [FromHeader(Name = "userName")] string userName)
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