using DocuSign.Models;

namespace DocuSign.DAL.Interfaces
{
	public interface IURIStorageMapper
	{
        public URI GetURIByName(string name);
        public string DeleteURLByName(string name);
        public void CreateURL(URI uri);
        public List<string> GetURLS();
    }
}

