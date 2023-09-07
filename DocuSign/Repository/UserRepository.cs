using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Linq;
using DocuSign.DAL;
using DocuSign.Interfaces;
using DocuSign.Models;

//for db calls
namespace DocuSign.Repository
{
	public class UserRepository : IUserRepository
	{
        private readonly IStorage _storage;
        private readonly string _idStoragePath;


        public UserRepository(IStorage storage)
        {
            _storage = storage;

            string tempPath = Path.GetTempPath();
            _idStoragePath = Path.Combine(tempPath, "SignServiceStorageIds");
            Directory.CreateDirectory(_idStoragePath);
            
        } //declare it's instance in program.cs! video 4(?) and end of video 5

        public void ConnectUser(string userName, string urlDomain)
        {
            throw new NotImplementedException();
        }

        public User CreateUser(string name, string lastName, string email)
        {
            //Process.Start(new ProcessStartInfo
            //{
            //    FileName = "https://www.ynet.co.il/home/0,7340,L-8,00.html",
            //    UseShellExecute = true
            //});

            User user = new(name, lastName, email);
            string id = _storage.AddData(JsonSerializer.SerializeToUtf8Bytes(user));
            string idFilePath = Path.Combine(_idStoragePath, name);
            File.WriteAllBytes(idFilePath, JsonSerializer.SerializeToUtf8Bytes(id));

            return user;
        }

        public void DeleteUser(string name)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string name)
        {
            //User emily = new("emily", "stel", "se");

            string idFilePath = Path.Combine(_idStoragePath, name);
            byte[] idBytes = File.ReadAllBytes(idFilePath);
            string id = JsonSerializer.Deserialize<string>(idBytes);

            //string id = _storage.AddData(JsonSerializer.SerializeToUtf8Bytes(emily));
            Console.WriteLine(id);
            //emily.Name = id;
            byte[] b = _storage.GetData(id);

            //maybe change storage file



            User deserializedUser = JsonSerializer.Deserialize<User>(b);
            
 
            return deserializedUser;
        }

        public List<string> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}

