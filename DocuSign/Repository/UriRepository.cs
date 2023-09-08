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
            if (uri != null) //uri exists
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

            else //uri does not exist
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

        public void DeleteUserUri(string userName, string uri)
        {
            throw new NotImplementedException();
        }

        public List<string> GetUserUris(string userName)
        {
            throw new NotImplementedException();
        }

        public void ConnectUser(string userName, string uriName)
        {
            throw new NotImplementedException();
            //Process.Start(new ProcessStartInfo
            //{
            //    FileName = "https://www.ynet.co.il/home/0,7340,L-8,00.html",
            //    UseShellExecute = true
            //});
        }
    }
}

