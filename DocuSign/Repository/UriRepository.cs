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
        private readonly IURIStorageMapper _URIStorageMapper;
        private readonly IStorageMapper _userStorageMapper;

        public URIRepository(IStorage storage, IURIStorageMapper uriStorageMapper, IStorageMapper userStorageMapper)
        {
            _storage = storage;
            _URIStorageMapper = uriStorageMapper;
            _userStorageMapper = userStorageMapper;
        }

        public URI AddUserUri(string userName, string uriName, string url)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                throw new InvalidOperationException("User does not exist");

            URI uri = _URIStorageMapper.GetURIByName(uriName);
            if (uri != null) //uri name exists
            {
                if (uri.URL == url)
                {
                    if (!uri.Users.Contains(userName))
                    {
                        uri.Users.Add(userName);
                    }
                    _URIStorageMapper.CreateURL(uri);
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

            else //uri name does not exist לבדוק אם קיים אכבר אצלך יוזר!!
            {
                uri = new(uriName, url);
                _URIStorageMapper.CreateURL(uri);

                string id = _userStorageMapper.GetIdByName(userName);
                byte[] userDataBytes = _storage.GetData(id);
                User deserializedUser = JsonSerializer.Deserialize<User>(userDataBytes);

                deserializedUser.Urls.Add(url);

                _storage.UpdateData(id, JsonSerializer.SerializeToUtf8Bytes(deserializedUser));
                return uri;
            }
        }

        public void DeleteUserUri(string userName, string uriName)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                throw new InvalidOperationException("User does not exist");

            URI uri = _URIStorageMapper.GetURIByName(uriName) ??
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
                _URIStorageMapper.DeleteURLByName(uriName);
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

        public void ConnectUser(string userName, string uriName)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                throw new InvalidOperationException("User does not exist");

            URI uri = _URIStorageMapper.GetURIByName(uriName) ??
                throw new InvalidOperationException("URI name does not exist");

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = JsonSerializer.Deserialize<User>(userDataBytes);

            if (!deserializedUser.Urls.Contains(uri.URL))
            {
                throw new InvalidOperationException("User does not have spicified url");
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.ynet.co.il/home/0,7340,L-8,00.html",
                UseShellExecute = true
            });
        }
    }
}

