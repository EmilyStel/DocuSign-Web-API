﻿using System.Diagnostics;
using System.Text.Json;
using DocuSign.Interfaces;
using DocuSign.Models;
using Domain.Exceptions;
using Domain.Constants;
using Domain.Interfaces;
using System.Web;

namespace BL.Repositories
{
    public class URIRepository : IURIRepository
    {
        private readonly IStorage _storage;
        private readonly IURIStorage _uriStorage;
        private readonly IUserStorageMapper _userStorageMapper;

        public URIRepository(IStorage storage, IURIStorage uriStorage, IUserStorageMapper userStorageMapper)
        {
            _storage = storage;
            _uriStorage = uriStorage;
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

            URI? uri = _uriStorage.GetUriByName(uriName);

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
                _uriStorage.CreateUri(uri);
                byte[] userDataBytes = _storage.GetData(userId);
                User deserializedUser = User.Deserialize(userDataBytes);

                deserializedUser.Uris.Add(uriName);

                _storage.UpdateData(userId, JsonSerializer.SerializeToUtf8Bytes(deserializedUser));
                
            }
            else
            {
                uri = new(uriName, url);
                uri.Users.Add(userName);
                _uriStorage.CreateUri(uri);

                string id = _userStorageMapper.GetIdByName(userName) ??
                    throw new NotFoundException(Entities.USER);

                byte[] userDataBytes = _storage.GetData(id);
                User deserializedUser = User.Deserialize(userDataBytes);

                deserializedUser.Uris.Add(uriName);

                _storage.UpdateData(id, JsonSerializer.SerializeToUtf8Bytes(deserializedUser));
            }

            return uri;
        }

        public void DeleteUserUri(string userName, string uriName)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                throw new NotFoundException(Entities.USER);

            URI uri = _uriStorage.GetUriByName(uriName) ??
                throw new NotFoundException(Entities.URI_NAME);

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = User.Deserialize(userDataBytes);

            if (!deserializedUser.Uris.Contains(uriName))
            {
                throw new NotFoundException(Entities.URI_NAME);
            }

            deserializedUser.Uris.Remove(uriName);
            _storage.UpdateData(userId, JsonSerializer.SerializeToUtf8Bytes(deserializedUser));

            if (uri.Users.Count == 1)
            {
                _uriStorage.DeleteUriByName(uriName);
            }
        }

        public List<string> GetUserUris(string userName)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
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

            return urls;
        }

        public void ConnectUser(string userName, string url)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                throw new NotFoundException(Entities.USER);

            string decodedUrl = HttpUtility.UrlDecode(url) ??
                throw new InvalidException(Entities.URL);

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

            if (!urls.Contains(decodedUrl))
            {
                throw new NotFoundException(Entities.URL);
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = decodedUrl,
                UseShellExecute = true
            });
        }
    }
}

