using DocuSign.Models;

namespace DocuSign.Interfaces
{
	public interface IURIStorage
	{
        public URI? GetUriByName(string name);
        public void DeleteUriByName(string name);
        public void CreateUri(URI uri);
    }
}

