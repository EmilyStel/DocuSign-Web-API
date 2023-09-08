namespace DocuSign.Models
{
	public class URI
	{
        public string Name { get; set; }
        public string URL { get; set; }
        public List<string> Users { get; set; }

        public URI(string name, string url)
		{
			Name = name;
			URL = url;
            Users = new List<string>();
        }
	}
}