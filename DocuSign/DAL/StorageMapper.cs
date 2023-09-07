﻿using System.Text.Json;
using DocuSign.Models;

namespace DocuSign.DAL
{
	public class StorageMapper : IStorageMapper
	{
        private readonly string _idStoragePath;

        public StorageMapper()
        {
            string tempPath = Path.GetTempPath();
            _idStoragePath = Path.Combine(tempPath, "SignServiceStorageIds");

            Directory.CreateDirectory(_idStoragePath);
        }

        public string GetIdByName(string name)
        {
            string idFilePath = Path.Combine(_idStoragePath, name);
            byte[] idBytes = File.ReadAllBytes(idFilePath);
            string userId = JsonSerializer.Deserialize<string>(idBytes);

            return userId;
        }

        public string DeleteIdByName(string name)
        {
            string id = GetIdByName(name);
            string idFilePath = Path.Combine(_idStoragePath, name);
            File.Delete(idFilePath);

            return id;
        }

        public void CreateUser(User user, string id)
        {
            string idFilePath = Path.Combine(_idStoragePath, user.Name);
            File.WriteAllBytes(idFilePath, JsonSerializer.SerializeToUtf8Bytes(id));
        }
       
        public string GetUser(string name)
        {
            string idFilePath = Path.Combine(_idStoragePath, name);
            byte[] idBytes = File.ReadAllBytes(idFilePath);
            string id = JsonSerializer.Deserialize<string>(idBytes);

            return id;
        }

        public List<string> GetUsers()
        {
            string[] files = Directory.GetFiles(_idStoragePath);
            List<string> userNames = files.Select(file => Path.GetFileName(file)).ToList();

            return userNames;
        }
    }
}