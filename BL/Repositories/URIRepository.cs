﻿using System.Diagnostics;
using System.Text.Json;
using DAL.Intefaces;
using DocuSign.Interfaces;
using DocuSign.Models;
using Domain.Exceptions;

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
                throw new NotFoundException("User");

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new InvalidException("URL");
            }

            URI? uri = _uriStorageMapper.GetUriByName(uriName);

            if (uri != null)
            {
                if(uri.Url != url)
                {
                    throw new AlreadyExistException("URI name");
                }

                if (uri.Users.Contains(userName))
                {
                    throw new AlreadyExistException("URI");
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
                    throw new NotFoundException("User");

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
                throw new NotFoundException("User");

            URI uri = _uriStorageMapper.GetUriByName(uriName) ??
                throw new NotFoundException("Uri name");

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = User.Deserialize(userDataBytes);

            if (!deserializedUser.Urls.Contains(uri.Url))
            {
                throw new NotFoundException("Uri name");
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
                    throw new NotFoundException("User");

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = User.Deserialize(userDataBytes);

            return deserializedUser.Urls;
        }

        public void ConnectUser(string userName, string url)
        {
            string userId = _userStorageMapper.GetIdByName(userName) ??
                throw new NotFoundException("User");

            byte[] userDataBytes = _storage.GetData(userId);
            User deserializedUser = User.Deserialize(userDataBytes);

            if (!deserializedUser.Urls.Contains(url))
            {
                throw new NotFoundException("Url");
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}
