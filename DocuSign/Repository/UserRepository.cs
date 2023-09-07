using System.Text.Json;
using DocuSign.DAL;
using DocuSign.Interfaces;
using DocuSign.Models;

namespace DocuSign.Repository
{
	public class UserRepository : IUserRepository
	{
        private readonly IStorage _storage;
        private readonly IStorageMapper _storageMapper;


        public UserRepository(IStorage storage, IStorageMapper storageMapper)
        {
            _storage = storage;
            _storageMapper = storageMapper;
        }

        public void ConnectUser(string userName, string urlDomain)
        {
            throw new NotImplementedException();
            //Process.Start(new ProcessStartInfo
            //{
            //    FileName = "https://www.ynet.co.il/home/0,7340,L-8,00.html",
            //    UseShellExecute = true
            //});
        }

        public User CreateUser(string name, string lastName, string email)
        {
            User user = new(name, lastName, email);
            string id = _storage.AddData(JsonSerializer.SerializeToUtf8Bytes(user));

            _storageMapper.CreateUser(user, id);

            return user;
        }

        public void DeleteUser(string name)
        {
            string userId = _storageMapper.DeleteIdByName(name);
            _storage.DeleteData(userId);
        }

        public User GetUser(string name)
        {
            string id = _storageMapper.GetUser(name);
            byte[] userDataBytes = _storage.GetData(id);
            User deserializedUser = JsonSerializer.Deserialize<User>(userDataBytes);

            return deserializedUser;
        }

        public List<string> GetUsers()
        {
            return _storageMapper.GetUsers();
        }
    }
}

