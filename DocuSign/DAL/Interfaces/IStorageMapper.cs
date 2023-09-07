using System;
using DocuSign.Models;

namespace DocuSign.DAL
{
	public interface IStorageMapper
	{
        public string GetIdByName(string name);
        public string DeleteIdByName(string name);
        public void CreateUser(User user, string id);
        public string GetUser(string name);
        public List<string> GetUsers();
    }
}

