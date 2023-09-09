using System.Diagnostics;
using System.Text.Json;
using DocuSign.DAL;
using DocuSign.DAL.Interfaces;
using DocuSign.Interfaces;
using DocuSign.Models;

namespace DocuSign.Repository
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
                throw new InvalidOperationException("User does not exist");


            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new InvalidOperationException("Url is invalid");
            }

            URI uri = _uriStorageMapper.GetURIByName(uriName);
            if (uri != null) //uri name exists
            {
                if (uri.URL == url)
                {
                    if (uri.Users.Contains(userName))
                    {
                        throw new InvalidOperationException("User already added this URI");
                    }

                    uri.Users.Add(userName);
                    _uriStorageMapper.CreateURL(uri);
                    byte[] userDataBytes = _storage.GetData(userId);
                    User deserializedUser = JsonSerializer.Deserialize<User>(userDataBytes);

                    deserializedUser.Urls.Add(url);

                    _storage.UpdateData(userId, JsonSerializer.SerializeToUtf8Bytes(deserializedUser));
                }
                if (uri.URL != url)
                {
                    throw new InvalidOperationException("URI name is in use");
                }
                return uri;
            }

            else
            {
                uri = new(uriName, url);
                _uriStorageMapper.CreateURL(uri);

                string id = _userStorageMapper.GetIdByName(userName);
                byte[] userDataBytes = _storage.GetData(id);
                User deserializedUser = JsonSerializer.Deserialize<User>(userDataBytes);

                deserializedUser.Urls.Add(url);

                // move it to DAL
                _storage.UpdateData(id, JsonSerializer.SerializeToUtf8Bytes(deserializedUser));
                return uri;
            }
        }

        public void DeleteUserUri(string userName, string uriName)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                throw new InvalidOperationException("User does not exist");

            URI uri = _uriStorageMapper.GetURIByName(uriName) ??
                throw new InvalidOperationException("URI name does not exist");

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = JsonSerializer.Deserialize<User>(userDataBytes);

            if (!deserializedUser.Urls.Contains(uri.URL))
            {
                throw new InvalidOperationException("User does not have spicified url");
            }

            deserializedUser.Urls.Remove(uri.URL);

            _storage.UpdateData(userId, JsonSerializer.SerializeToUtf8Bytes(deserializedUser));

            if (uri.Users.Count == 0)
            {
                _uriStorageMapper.DeleteURLByName(uriName);
            }
        }
         
        public List<string> GetUserUris(string userName)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                    throw new InvalidOperationException("User does not exist");

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = JsonSerializer.Deserialize<User>(userDataBytes);

            return deserializedUser.Urls;
        }

        public void ConnectUser(string userName, string url)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                throw new InvalidOperationException("User does not exist");

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = JsonSerializer.Deserialize<User>(userDataBytes);

            if (!deserializedUser.Urls.Contains(url))
            {
                throw new InvalidOperationException("User does not have spicified url");
            }

            // add site validator
            Process.Start(new ProcessStartInfo
            {
                //FileName = "https://www.ynet.co.il/home/0,7340,L-8,00.html",
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}

