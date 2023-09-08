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
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        //hanDALe so that urllist will not appear in request body
        public IActionResult AddUserUri([FromBody] AddURIDto body)
        {
            //// make validation if yri name already exsits and return suitable response
            try
            {
                URI uri = _uriRepository.AddUserUri(body.UserName, body.Name, body.URL);
                return Ok(uri);

            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }

        //[HttpPost]
        //[ProducesResponseType(200, Type = typeof(User))]
        //[ProducesResponseType(400)]
        ////hanDALe so that urllist will not appear in request body
        //public IActionResult ConnectUser([FromBody] ConnectDto URI)
        //{
        //    //// make validation if user already exsits and return suitable response
        //    ///

        //    _uriRepository.ConnectUser(URI.UserName, URI.URL);
        //    return Ok();
        //}
    }
}

