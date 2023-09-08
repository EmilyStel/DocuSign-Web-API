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

        public User CreateUser(string name, string lastName, string email)
        {
            string userId = _storageMapper.GetIdByName(name);
            if (userId != null)
            {
                throw new InvalidOperationException("User name is already in use");
            }

            User user = new(name, lastName, email);
            userId = _storage.AddData(JsonSerializer.SerializeToUtf8Bytes(user));

            _storageMapper.CreateUser(user, userId);

            return user;
        }

        public void DeleteUser(string name)
        {
            string userId = _storageMapper.DeleteIdByName(name) ??
                throw new InvalidOperationException("User does not exist");

            _storage.DeleteData(userId);
        }

        public User GetUser(string name)
        {
            string userId = _storageMapper.GetIdByName(name) ??
                throw new InvalidOperationException("User does not exist");

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = JsonSerializer.Deserialize<User>(userDataBytes);

            return deserializedUser;
        }

        public List<string> GetUsers()
        {
            return _storageMapper.GetUsers();
        }
    }
}

