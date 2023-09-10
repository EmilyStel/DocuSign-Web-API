using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DocuSign.Interfaces;
using DocuSign.Models;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Interfaces;

namespace BL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IStorage _storage;
        private readonly IUserStorageMapper _userStorageMapper;


        public UserRepository(IStorage storage, IUserStorageMapper storageMapper)
        {
            _storage = storage;
            _userStorageMapper = storageMapper;
        }

        public User CreateUser(string name, string lastName, string email)
        {
            string? userId = _userStorageMapper.GetIdByName(name);

            if (userId != null)
            {
                throw new AlreadyExistException(Entities.USER_NAME);
            }

            if (!new EmailAddressAttribute().IsValid(email))
            {
                throw new InvalidException(Entities.EMAIL);
            }

            User user = new(name, lastName, email);
            userId = _storage.AddData(JsonSerializer.SerializeToUtf8Bytes(user));

            _userStorageMapper.CreateUser(user, userId);

            return user;
        }

        public void DeleteUser(string name)
        {
            string userId = _userStorageMapper.DeleteIdByName(name) ??
                throw new NotFoundException(Entities.USER);

            _storage.DeleteData(userId);
        }

        public User GetUser(string name)
        {
            string userId = _userStorageMapper.GetIdByName(name) ??
                throw new NotFoundException(Entities.USER);

            byte[] userDataBytes = _storage.GetData(userId);

            return User.Deserialize(userDataBytes);
        }

        public List<string> GetUsers()
        {
            return _userStorageMapper.GetUsers();
        }
    }
}

