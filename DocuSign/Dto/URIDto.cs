using System;
namespace DocuSign.Dto
{
	public class URIDto
	{
        public required string UriName { get; set; }
        public required string URL { get; set; }

        public URIDto(string URIName, string url)
		{
            UriName = URIName;
            URL = url;
        }
	}
}

