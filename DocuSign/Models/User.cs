using System;
namespace DocuSign.Models
{
	public class User
	{
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Urls { get; set; } // maybe add to cunstructor, access type


        public User(string name, string lastName, string email)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            Urls = new List<string>();
        }
    }
}

