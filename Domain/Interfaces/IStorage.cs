namespace Domain.Interfaces
{
    public interface IStorage
    {
        string AddData(byte[] data);
        void UpdateData(string dataID, byte[] newData);
        void DeleteData(string dataID);
        byte[] GetData(string dataID);
    }
}

