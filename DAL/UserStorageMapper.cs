using System.Text.Json;
using DocuSign.Interfaces;
using DocuSign.Models;

namespace DAL
{
    /// <summary>
    /// The UserStorageMapper class provides methods to manage IDs of User files provided by the <see cref="Storage"/> class.
    /// </summary>
    public class UserStorageMapper : IUserStorageMapper
    {
        private readonly string _idStoragePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStorageMapper"/> class.
        /// It creates a directory for mapping usernames to IDs under the system's temporary folder.
        /// </summary>
        public UserStorageMapper()
        {
            string tempPath = Path.GetTempPath();
            _idStoragePath = Path.Combine(tempPath, "SignServiceStorageIds");

            Directory.CreateDirectory(_idStoragePath);
        }


        /// <summary>
        /// Creates a new mapping between a username and their ID.
        /// </summary>
        /// <param name="user">The user object containing the user's name.</param>
        /// <param name="id">The user's ID to be stored.</param>
        public void CreateUser(User user, string id)
        {
            string idFilePath = Path.Combine(_idStoragePath, user.Name);

            File.WriteAllBytes(idFilePath, JsonSerializer.SerializeToUtf8Bytes(id));
        }

        /// <summary>
        /// Deletes a User mapping entry by their name.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>The deleted user's ID if found; otherwise, returns null.</returns>
        public string? DeleteIdByName(string name)
        {
            string? userId = GetIdByName(name);
            string idFilePath = Path.Combine(_idStoragePath, name);

            File.Delete(idFilePath);

            return userId;
        }

        /// <summary>
        /// Retrieves the user ID associated with a username.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>The user's ID if found; otherwise, returns null.</returns>
        public string? GetIdByName(string name)
        {
            string idFilePath = Path.Combine(_idStoragePath, name);

            if (File.Exists(idFilePath))
            {
                byte[] idBytes = File.ReadAllBytes(idFilePath);

                return JsonSerializer.Deserialize<string>(idBytes);
            }

            return null;
        }

        /// <summary>
        /// Retrieves a list of usernames.
        /// </summary>
        /// <returns>A list of user names.</returns>
        public List<string> GetUsers()
        {
            string[] files = Directory.GetFiles(_idStoragePath);

            return files.Select(file => Path.GetFileName(file)).ToList();
        }
    }
}

