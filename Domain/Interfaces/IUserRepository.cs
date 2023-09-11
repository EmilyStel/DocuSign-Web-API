using DocuSign.Models;
using Domain.Models;

namespace DocuSign.Interfaces
{
	public interface IUserRepository
	{
        UserResponse GetUser(string Name);
        List<string> GetUsers();
        UserResponse CreateUser(string name, string lastName, string email);
        void DeleteUser(string name);
    }
}

