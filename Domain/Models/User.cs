using System.Text.Json;
using Domain.Constants;
using Domain.Exceptions;

namespace DocuSign.Models
{
	public class User
	{
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Urls { get; set; }

        public User(string name, string lastName, string email)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            Urls = new List<string>();
        }

        public static User Deserialize(byte[] userDataBytes)
        {
            return JsonSerializer.Deserialize<User>(userDataBytes) ??
                throw new InvalidException(Entities.USER);

        }
    }
}

