using System.Diagnostics;
using System.Text.Json;
using DAL.Intefaces;
using DocuSign.Interfaces;
using DocuSign.Models;
using Domain.Exceptions;
using Domain.Constants;

namespace BL.Repositories
{
    public class URIRepository : IURIRepository
    {
        private readonly IStorage _storage;
        private readonly IURIStorageMapper _uriStorageMapper;
        private readonly IUserStorageMapper _userStorageMapper;

        public URIRepository(IStorage storage, IURIStorageMapper uriStorageMapper, IUserStorageMapper userStorageMapper)
        {
            _storage = storage;
            _uriStorageMapper = uriStorageMapper;
            _userStorageMapper = userStorageMapper;
        }

        public URI AddUserUri(string userName, string uriName, string url)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                throw new NotFoundException(Entities.USER);

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new InvalidException(Entities.URL);
            }

            URI? uri = _uriStorageMapper.GetUriByName(uriName);

            if (uri != null)
            {
                if(uri.Url != url)
                {
                    throw new AlreadyExistException(Entities.URI_NAME);
                }

                if (uri.Users.Contains(userName))
                {
                    throw new AlreadyExistException(Entities.URI);
                }

                uri.Users.Add(userName);
                _uriStorageMapper.CreateUri(uri);
                byte[] userDataBytes = _storage.GetData(userId);
                User deserializedUser = User.Deserialize(userDataBytes);

                deserializedUser.Urls.Add(url);

                _storage.UpdateData(userId, JsonSerializer.SerializeToUtf8Bytes(deserializedUser));
                
            }
            else
            {
                uri = new(uriName, url);
                _uriStorageMapper.CreateUri(uri);

                string id = _userStorageMapper.GetIdByName(userName) ??
                    throw new NotFoundException(Entities.USER);

                byte[] userDataBytes = _storage.GetData(id);
                User deserializedUser = User.Deserialize(userDataBytes);

                deserializedUser.Urls.Add(url);

                _storage.UpdateData(id, JsonSerializer.SerializeToUtf8Bytes(deserializedUser));
            }

            return uri;
        }

        public void DeleteUserUri(string userName, string uriName)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                throw new NotFoundException(Entities.USER);

            URI uri = _uriStorageMapper.GetUriByName(uriName) ??
                throw new NotFoundException(Entities.URI_NAME);

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = User.Deserialize(userDataBytes);

            if (!deserializedUser.Urls.Contains(uri.Url))
            {
                throw new NotFoundException(Entities.URI_NAME);
            }

            deserializedUser.Urls.Remove(uri.Url);

            _storage.UpdateData(userId, JsonSerializer.SerializeToUtf8Bytes(deserializedUser));

            if (uri.Users.Count == 0)
            {
                _uriStorageMapper.DeleteUriByName(uriName);
            }
        }

        public List<string> GetUserUris(string userName)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                    throw new NotFoundException(Entities.USER);

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = User.Deserialize(userDataBytes);

            return deserializedUser.Urls;
        }

        public void ConnectUser(string userName, string url)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                throw new NotFoundException(Entities.USER);

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = User.Deserialize(userDataBytes);

            if (!deserializedUser.Urls.Contains(url))
            {
                throw new NotFoundException(Entities.URL);
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}

