using System.Text.Json;
using DocuSign.Interfaces;
using DocuSign.Models;

namespace DAL
{
    /// <summary>
    /// The <see cref="URIStorage"/> class provides methods to manage URI files.
    /// </summary>
    public class URIStorage : IURIStorage
    {
        private readonly string _uriStoragePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="URIStorage"/> class.
        /// It creates a directory for storing URIs by their names.
        /// </summary>
        public URIStorage()
        {
            string tempPath = Path.GetTempPath();
            _uriStoragePath = Path.Combine(tempPath, "SignServiceStorageURLs");

            Directory.CreateDirectory(_uriStoragePath);
        }

        /// <summary>
        /// Creates a new URI file called <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">The URI object to be stored.</param>
        public void CreateUri(URI uri)
        {
            string uriFilePath = Path.Combine(_uriStoragePath, uri.Name);

            File.WriteAllBytes(uriFilePath, JsonSerializer.SerializeToUtf8Bytes(uri));
        }

        /// <summary>
        /// Deletes a URI by its name.
        /// </summary>
        /// <param name="uriName">The name of the URI to be deleted.</param>
        public void DeleteUriByName(string uriName)
        {
            string uriFilePath = Path.Combine(_uriStoragePath, uriName);

            File.Delete(uriFilePath);
        }

        /// <summary>
        /// Retrieves a URI by its name.
        /// </summary>
        /// <param name="uriName">The name of the URI to retrieve.</param>
        /// <returns>The URI object if found; otherwise, returns null.</returns>
        public URI? GetUriByName(string uriName)
        {
            string uriFilePath = Path.Combine(_uriStoragePath, uriName);

            if (File.Exists(uriFilePath))
            {
                byte[] idBytes = File.ReadAllBytes(uriFilePath);

                return JsonSerializer.Deserialize<URI>(idBytes);
            }

            return null;
        }
    }
}

