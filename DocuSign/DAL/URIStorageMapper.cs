using System;
using System.Text.Json;
using DocuSign.DAL.Interfaces;
using DocuSign.Models;

namespace DocuSign.DAL
{
	public class URIStorageMapper : IURIStorageMapper
	{
        private readonly string _URLStoragePath;

        public URIStorageMapper()
		{
            string tempPath = Path.GetTempPath();
            _URLStoragePath = Path.Combine(tempPath, "SignServiceStorageURLs");

            Directory.CreateDirectory(_URLStoragePath);
        }

        public void CreateURL(URI uri)
        {
            string URLFilePath = Path.Combine(_URLStoragePath, uri.Name);
            File.WriteAllBytes(URLFilePath, JsonSerializer.SerializeToUtf8Bytes(uri));
        }

        public string DeleteURLByName(string name)
        {
            throw new NotImplementedException();
        }

        public URI GetURIByName(string name)
        {
            string URIFilePath = Path.Combine(_URLStoragePath, name);
            if (File.Exists(URIFilePath))
            {
                byte[] idBytes = File.ReadAllBytes(URIFilePath);
                URI uri = JsonSerializer.Deserialize<URI>(idBytes);
                return uri;
            }

            return null;
        }

        public List<string> GetURLS()
        {
            throw new NotImplementedException();
        }
    }
}

