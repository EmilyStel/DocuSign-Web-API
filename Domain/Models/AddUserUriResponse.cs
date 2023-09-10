namespace DocuSign.Models
{
	public class AddUserUriResponse
	{
        public string Url { get; set; }
        public string UserName { get; set; }

        public AddUserUriResponse(string url, string userName)
        {
            Url = url;
            UserName = userName;
        }
    }
}

