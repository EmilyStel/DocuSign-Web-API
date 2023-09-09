using System.Text.Json;
using DocuSign.Models;

namespace DocuSign.DAL
{
	public class UserStorageMapper : IUserStorageMapper
    {
        private readonly string _idStoragePath;

        public UserStorageMapper()
        {
            string tempPath = Path.GetTempPath();
            _idStoragePath = Path.Combine(tempPath, "SignServiceStorageIds");

            Directory.CreateDirectory(_idStoragePath);
        }

        public string GetIdByName(string name)
        {
            string idFilePath = Path.Combine(_idStoragePath, name);

            if (File.Exists(idFilePath))
            {
                byte[] idBytes = File.ReadAllBytes(idFilePath);
                string userId = JsonSerializer.Deserialize<string>(idBytes);

                return userId;
            }

            return null;
        }

        public string DeleteIdByName(string name)
        {
            string userId = GetIdByName(name);
            string idFilePath = Path.Combine(_idStoragePath, name);
            File.Delete(idFilePath);

            return userId;
        }

        public void CreateUser(User user, string id)
        {
            string idFilePath = Path.Combine(_idStoragePath, user.Name);
            File.WriteAllBytes(idFilePath, JsonSerializer.SerializeToUtf8Bytes(id));
        }

        public List<string> GetUsers()
        {
            string[] files = Directory.GetFiles(_idStoragePath);
            List<string> userNames = files.Select(file => Path.GetFileName(file)).ToList();

            return userNames;
        }
    }
}