namespace Domain.Models
{
    public class UserResponse
	{

        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Urls { get; set; }

        public UserResponse(string name, string lastName, string email)
		{
            Name = name;
            LastName = lastName;
            Email = email;
            Urls = new List<string>();
        }

        public UserResponse(string name, string lastName, string email, List<string> urls)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            Urls = urls;
        }
    }
}