using System;
namespace DocuSign.Dto
{
	public class AddURIDto
	{
        public required string UserName { get; set; }
        public required string Name { get; set; }
        public required string URL { get; set; }
    }
}