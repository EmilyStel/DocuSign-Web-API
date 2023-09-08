using DocuSign.Models;

namespace DocuSign.Interfaces
{
	public interface IUserRepository
	{
		User GetUser(string Name);
        List<string> GetUsers();
        User CreateUser(string name, string lastName, string email);
        void DeleteUser(string name);
    }
}

