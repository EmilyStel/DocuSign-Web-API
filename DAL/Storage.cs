using DAL.Intefaces;

namespace DAL
{
    public class Storage : IStorage
    {
        public Storage()
        {
            string tempPath = Path.GetTempPath();
            m_storagePath = Path.Combine(tempPath, "SignServiceStorage");

            Directory.CreateDirectory(m_storagePath);
        }

        private readonly string m_storagePath = null;

        public string AddData(byte[] data)
        {
            string storageId = Path.GetRandomFileName();
            File.WriteAllBytes(Path.Combine(m_storagePath, storageId), data);

            return storageId;
        }

        public void UpdateData(string dataID, byte[] newData)
        {
            File.WriteAllBytes(Path.Combine(m_storagePath, dataID), newData);
        }

        public void DeleteData(string dataID)
        {
            File.Delete(Path.Combine(m_storagePath, dataID));
        }

        public byte[] GetData(string dataID)
        {
            try
            {
                byte[] data = File.ReadAllBytes(Path.Combine(m_storagePath, dataID));
                return data;
            }
            catch (FileNotFoundException fex)
            {
                return null;
            }
        }
    }
}

