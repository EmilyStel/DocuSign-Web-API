using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using DocuSign.Interfaces;
using DocuSign.Models;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;

namespace BL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IStorage _storage;
        private readonly IUserStorageMapper _userStorageMapper;
        private readonly IURIStorage _uriStorage;

        public UserRepository(IStorage storage, IUserStorageMapper userStorageMapper, IURIStorage uriStorage)
        {
            _storage = storage;
            _userStorageMapper = userStorageMapper;
            _uriStorage = uriStorage;

        }

        public UserResponse CreateUser(string name, string lastName, string email)
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

            return new(name, lastName, email);
        }

        public void DeleteUser(string name)
        {
            string userId = _userStorageMapper.DeleteIdByName(name) ??
                throw new NotFoundException(Entities.USER);

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = User.Deserialize(userDataBytes);
            List<URI?> uris = deserializedUser.Uris.Select(_uriStorage.GetUriByName).ToList();

            foreach (URI? uri in uris)
            {
                if(uri == null)
                {
                    throw new InvalidException(Entities.URI);
                }
                if (uri.Users.Count == 1 )
                {
                    _uriStorage.DeleteUriByName(uri.Name);
                }
            }

            _storage.DeleteData(userId);
        }

        public UserResponse GetUser(string name)
        {
            string userId = _userStorageMapper.GetIdByName(name) ??
                throw new NotFoundException(Entities.USER);

            byte[] userDataBytes = _storage.GetData(userId);

            User deserializedUser = User.Deserialize(userDataBytes);
            List<URI?> uris = deserializedUser.Uris.Select(_uriStorage.GetUriByName).ToList();
            List<string> urls = new List<string>();

            foreach (URI? uri in uris)
            {
                if (uri == null)
                {
                    throw new InvalidException(Entities.URI);
                }
                urls.Add(uri.Url);
            }

            return new UserResponse(deserializedUser.Name, deserializedUser.LastName, deserializedUser.Email, urls);
        }

        public List<string> GetUsers()
        {
            return _userStorageMapper.GetUsers();
        }
    }
}

