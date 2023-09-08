using DocuSign.Models;

namespace DocuSign.Interfaces
{
	public interface IURIRepository
    {
        List<string> GetUserUris(string userName);
        URI AddUserUri(string userName, string uriName, string url);
        void DeleteUserUri(string userName, string uri);
        public void ConnectUser(string userName, string uriName);
    }
}