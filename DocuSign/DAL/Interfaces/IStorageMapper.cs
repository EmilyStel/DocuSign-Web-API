using DocuSign.Models;

namespace DocuSign.DAL
{
	public interface IStorageMapper
	{
        public string GetIdByName(string name);
        public string DeleteIdByName(string name);
        public void CreateUser(User user, string id);
        public List<string> GetUsers();
    }
}

