namespace DocuSign.Models
{
	public class URI
	{
        public string Name { get; set; }
        public string Url { get; set; }
        public List<string> Users { get; set; }

        public URI(string name, string url)
        {
            Name = name;
            Url = url;
            Users = new List<string>();
        }
    }
}

