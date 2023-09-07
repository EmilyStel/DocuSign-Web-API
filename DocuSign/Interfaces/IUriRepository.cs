using System;
namespace DocuSign.Interfaces
{
	public interface IUriRepository
    {
        List<string> GetUserUris(string userName);

        Uri AddUserUri(string userName, string uri, string url);

        void DeleteUserUri(string userName, string uri);
    }
}